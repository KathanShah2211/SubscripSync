import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';
import { MatCardModule } from '@angular/material/card';
import { SubscriptionService } from '../../services/subscription.service';
import { Subscription } from '../../models/subscription.model';

@Component({
    selector: 'app-analytics',
    standalone: true,
    imports: [CommonModule, MatCardModule, BaseChartDirective],
    templateUrl: './analytics.component.html',
    styleUrl: './analytics.component.css'
})
export class AnalyticsComponent implements OnInit {
    userId = '3fa85f64-5717-4562-b3fc-2c963f66afa6'; // Hardcoded for demo

    // Pie Chart - Spending by Category
    public pieChartOptions: ChartConfiguration['options'] = {
        responsive: true,
        plugins: {
            legend: {
                display: true,
                position: 'top',
            },
        }
    };
    public pieChartData: ChartData<'pie', number[], string | string[]> = {
        labels: [],
        datasets: [{ data: [] }]
    };
    public pieChartType: ChartType = 'pie';

    // Bar Chart - Monthly Costs
    public barChartOptions: ChartConfiguration['options'] = {
        responsive: true,
    };
    public barChartData: ChartData<'bar'> = {
        labels: ['Monthly Cost'],
        datasets: [
            { data: [], label: 'Total Spending' }
        ]
    };
    public barChartType: ChartType = 'bar';

    constructor(private subscriptionService: SubscriptionService) { }

    ngOnInit(): void {
        this.loadData();
    }

    loadData() {
        this.subscriptionService.getUserSubscriptions(this.userId).subscribe(subs => {
            this.processCategoryData(subs);
            this.processMonthlyData(subs);
        });
    }

    processCategoryData(subs: Subscription[]) {
        const categoryMap = new Map<string, number>();

        subs.forEach(sub => {
            // Normalize cost to monthly for fair comparison
            let monthlyCost = sub.amount;
            if (sub.billingCycle === 0) monthlyCost *= 4; // Weekly
            if (sub.billingCycle === 2) monthlyCost /= 12; // Yearly

            const current = categoryMap.get(sub.category) || 0;
            categoryMap.set(sub.category, current + monthlyCost);
        });

        this.pieChartData = {
            labels: Array.from(categoryMap.keys()),
            datasets: [{
                data: Array.from(categoryMap.values()),
                backgroundColor: [
                    '#FF6384',
                    '#36A2EB',
                    '#FFCE56',
                    '#4BC0C0',
                    '#9966FF'
                ]
            }]
        };
    }

    processMonthlyData(subs: Subscription[]) {
        let totalMonthly = 0;
        subs.forEach(sub => {
            let monthlyCost = sub.amount;
            if (sub.billingCycle === 0) monthlyCost *= 4;
            if (sub.billingCycle === 2) monthlyCost /= 12;
            totalMonthly += monthlyCost;
        });

        this.barChartData = {
            labels: ['Total Monthly Spending'],
            datasets: [
                { data: [totalMonthly], label: 'Cost (USD)', backgroundColor: '#36A2EB' }
            ]
        };
    }
}
