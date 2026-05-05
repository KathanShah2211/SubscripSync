import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { SubscriptionService } from '../../services/subscription.service';
import { AuthService } from '../../services/auth.service';
import { Subscription } from '../../models/subscription.model';

@Component({
    selector: 'app-analytics',
    standalone: true,
    imports: [CommonModule, MatCardModule, MatIconModule, BaseChartDirective],
    templateUrl: './analytics.component.html',
    styleUrl: './analytics.component.css'
})
export class AnalyticsComponent implements OnInit {
    userId = '';
    totalMonthly: number = 0;
    categoryCount: number = 0;
    avgPerSub: number = 0;
    displayCurrency: string = 'USD';

    public pieChartOptions: any = {
        responsive: true,
        maintainAspectRatio: false,
        cutout: '65%',
        plugins: {
            legend: {
                display: true,
                position: 'bottom',
                labels: {
                    color: '#a09bb5',
                    font: {
                        family: 'Inter',
                        size: 12,
                        weight: 500
                    },
                    padding: 16,
                    usePointStyle: true,
                    pointStyleWidth: 10,
                }
            },
            tooltip: {
                backgroundColor: 'rgba(19, 16, 30, 0.95)',
                titleColor: '#f0ecf9',
                bodyColor: '#a09bb5',
                borderColor: 'rgba(255,255,255,0.06)',
                borderWidth: 1,
                cornerRadius: 10,
                padding: 12,
                titleFont: { family: 'Inter', weight: 600 },
                bodyFont: { family: 'Inter' },
            }
        },
    };
    public pieChartData: ChartData<'doughnut', number[], string | string[]> = {
        labels: [],
        datasets: [{ data: [] }]
    };
    public pieChartType: ChartType = 'doughnut';

    // Bar Chart - Monthly Costs
    public barChartOptions: ChartConfiguration['options'] = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                display: false,
            },
            tooltip: {
                backgroundColor: 'rgba(19, 16, 30, 0.95)',
                titleColor: '#f0ecf9',
                bodyColor: '#a09bb5',
                borderColor: 'rgba(255,255,255,0.06)',
                borderWidth: 1,
                cornerRadius: 10,
                padding: 12,
                titleFont: { family: 'Inter', weight: 600 },
                bodyFont: { family: 'Inter' },
            }
        },
        scales: {
            x: {
                ticks: {
                    color: '#6b6580',
                    font: { family: 'Inter', size: 12 }
                },
                grid: { color: 'rgba(255,255,255,0.03)' },
                border: { color: 'rgba(255,255,255,0.06)' },
            },
            y: {
                ticks: {
                    color: '#6b6580',
                    font: { family: 'Inter', size: 12 }
                },
                grid: { color: 'rgba(255,255,255,0.03)' },
                border: { color: 'rgba(255,255,255,0.06)' },
            }
        }
    };
    public barChartData: ChartData<'bar'> = {
        labels: ['Monthly Cost'],
        datasets: [
            { data: [], label: 'Total Spending' }
        ]
    };
    public barChartType: ChartType = 'bar';

    constructor(
        private subscriptionService: SubscriptionService,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        const userId = this.authService.getUserId();
        if (userId) {
            this.userId = userId;
            this.loadData();
        }
    }

    loadData() {
        this.subscriptionService.getUserSubscriptions(this.userId).subscribe(subs => {
            this.processCategoryData(subs);
            this.processMonthlyData(subs);
            this.computeSummary(subs);
        });
    }

    computeSummary(subs: Subscription[]) {
        if (subs.length > 0) {
            this.displayCurrency = subs[0].currency;
        }

        let total = 0;
        const categories = new Set<string>();
        subs.forEach(sub => {
            let monthlyCost = sub.amount;
            if (sub.billingCycle === 0) monthlyCost *= 4;
            if (sub.billingCycle === 2) monthlyCost /= 12;
            total += monthlyCost;
            categories.add(sub.category);
        });

        this.totalMonthly = total;
        this.categoryCount = categories.size;
        this.avgPerSub = subs.length > 0 ? total / subs.length : 0;
    }

    processCategoryData(subs: Subscription[]) {
        const categoryMap = new Map<string, number>();

        subs.forEach(sub => {
            let monthlyCost = sub.amount;
            if (sub.billingCycle === 0) monthlyCost *= 4;
            if (sub.billingCycle === 2) monthlyCost /= 12;

            const current = categoryMap.get(sub.category) || 0;
            categoryMap.set(sub.category, current + monthlyCost);
        });

        const darkChartColors = [
            '#e879f9', // Entertainment - fuchsia
            '#60a5fa', // Software - blue
            '#34d399', // Utilities - emerald
            '#fbbf24', // Education - amber
            '#f97316', // Productivity - orange
            '#a78bfa', // Other - violet
        ];

        this.pieChartData = {
            labels: Array.from(categoryMap.keys()),
            datasets: [{
                data: Array.from(categoryMap.values()),
                backgroundColor: darkChartColors.slice(0, categoryMap.size),
                borderWidth: 0,
                hoverOffset: 8,
            }]
        };
    }

    processMonthlyData(subs: Subscription[]) {
        const categoryMap = new Map<string, number>();
        let preferredCurrency = 'USD';

        if (subs.length > 0) {
            preferredCurrency = subs[0].currency;
        }

        subs.forEach(sub => {
            let monthlyCost = sub.amount;
            if (sub.billingCycle === 0) monthlyCost *= 4;
            if (sub.billingCycle === 2) monthlyCost /= 12;
            const current = categoryMap.get(sub.category) || 0;
            categoryMap.set(sub.category, current + monthlyCost);
        });

        const darkBarColors = [
            '#8c3fff',
            '#6d1bf0',
            '#5a0fd4',
            '#4808b0',
            '#a78bfa',
            '#c084fc',
        ];

        this.barChartData = {
            labels: Array.from(categoryMap.keys()),
            datasets: [{
                data: Array.from(categoryMap.values()),
                label: `Cost (${preferredCurrency})`,
                backgroundColor: darkBarColors.slice(0, categoryMap.size),
                borderRadius: 8,
                borderSkipped: false,
            }]
        };
    }
}
