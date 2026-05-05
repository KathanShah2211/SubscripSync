import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { SubscriptionService } from '../../services/subscription.service';
import { AuthService } from '../../services/auth.service';
import { Subscription } from '../../models/subscription.model';
import Swal from 'sweetalert2';

import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, MatDividerModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  subscriptions: Subscription[] = [];
  userId: string | null = null;
  totalMonthlyCost: number = 0;
  preferredCurrency: string = 'USD';
  activeCount: number = 0;
  nextRenewal: Date | null = null;

  constructor(
    private subscriptionService: SubscriptionService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.userId = this.authService.getUserId();
    if (this.userId) {
      this.loadSubscriptions();
    }
  }

  loadSubscriptions() {
    if (!this.userId) return;
    this.subscriptionService.getUserSubscriptions(this.userId).subscribe({
      next: (data) => {
        this.subscriptions = data;
        this.calculateStats();
      },
      error: (err) => console.error('Error fetching subscriptions', err)
    });
  }

  calculateStats() {
    this.totalMonthlyCost = 0;
    this.activeCount = this.subscriptions.filter(s => s.isActive).length;

    if (this.subscriptions.length > 0) {
      this.preferredCurrency = this.subscriptions[0].currency;

      // Find nearest renewal
      const sorted = [...this.subscriptions]
        .filter(s => s.isActive)
        .sort((a, b) => new Date(a.nextPaymentDate).getTime() - new Date(b.nextPaymentDate).getTime());
      this.nextRenewal = sorted.length > 0 ? new Date(sorted[0].nextPaymentDate) : null;
    }

    this.subscriptions.forEach(sub => {
      let monthlyCost = sub.amount;
      if (sub.billingCycle === 0) monthlyCost *= 4; // Weekly to Monthly
      if (sub.billingCycle === 2) monthlyCost /= 12; // Yearly to Monthly
      this.totalMonthlyCost += monthlyCost;
    });
  }

  getCategoryIcon(category: string): string {
    const icons: { [key: string]: string } = {
      'Entertainment': 'movie',
      'Software': 'code',
      'Utilities': 'electrical_services',
      'Education': 'school',
      'Productivity': 'rocket_launch',
      'Other': 'category'
    };
    return icons[category] || 'category';
  }

  getCategoryColor(category: string): string {
    const colors: { [key: string]: string } = {
      'Entertainment': '#e879f9',
      'Software': '#60a5fa',
      'Utilities': '#34d399',
      'Education': '#fbbf24',
      'Productivity': '#f97316',
      'Other': '#a78bfa'
    };
    return colors[category] || '#a78bfa';
  }

  getCategoryBg(category: string): string {
    const colors: { [key: string]: string } = {
      'Entertainment': 'rgba(232, 121, 249, 0.12)',
      'Software': 'rgba(96, 165, 250, 0.12)',
      'Utilities': 'rgba(52, 211, 153, 0.12)',
      'Education': 'rgba(251, 191, 36, 0.12)',
      'Productivity': 'rgba(249, 115, 22, 0.12)',
      'Other': 'rgba(167, 139, 250, 0.12)'
    };
    return colors[category] || 'rgba(167, 139, 250, 0.12)';
  }

  async editSubscription(sub: Subscription) {
    const { value: formValues } = await Swal.fire({
      title: 'Edit Subscription',
      html: `
        <input id="swal-name" class="swal2-input" placeholder="Name" value="${sub.name}">
        <input id="swal-amount" type="number" class="swal2-input" placeholder="Amount" value="${sub.amount}">
        <select id="swal-currency" class="swal2-input">
          <option value="USD" ${sub.currency === 'USD' ? 'selected' : ''}>USD</option>
          <option value="EUR" ${sub.currency === 'EUR' ? 'selected' : ''}>EUR</option>
          <option value="GBP" ${sub.currency === 'GBP' ? 'selected' : ''}>GBP</option>
          <option value="INR" ${sub.currency === 'INR' ? 'selected' : ''}>INR</option>
          <option value="JPY" ${sub.currency === 'JPY' ? 'selected' : ''}>JPY</option>
          <option value="CAD" ${sub.currency === 'CAD' ? 'selected' : ''}>CAD</option>
          <option value="AUD" ${sub.currency === 'AUD' ? 'selected' : ''}>AUD</option>
        </select>
        <select id="swal-cycle" class="swal2-input">
          <option value="1" ${sub.billingCycle === 1 ? 'selected' : ''}>Monthly</option>
          <option value="2" ${sub.billingCycle === 2 ? 'selected' : ''}>Yearly</option>
          <option value="0" ${sub.billingCycle === 0 ? 'selected' : ''}>Weekly</option>
        </select>
        <input id="swal-date" type="date" class="swal2-input" value="${new Date(sub.nextPaymentDate).toISOString().split('T')[0]}">
        <select id="swal-category" class="swal2-input">
          <option value="Entertainment" ${sub.category === 'Entertainment' ? 'selected' : ''}>Entertainment</option>
          <option value="Software" ${sub.category === 'Software' ? 'selected' : ''}>Software</option>
          <option value="Utilities" ${sub.category === 'Utilities' ? 'selected' : ''}>Utilities</option>
          <option value="Education" ${sub.category === 'Education' ? 'selected' : ''}>Education</option>
          <option value="Productivity" ${sub.category === 'Productivity' ? 'selected' : ''}>Productivity</option>
          <option value="Other" ${sub.category === 'Other' ? 'selected' : ''}>Other</option>
        </select>
      `,
      focusConfirm: false,
      showCancelButton: true,
      preConfirm: () => {
        return {
          id: sub.id,
          userId: this.userId, // Ensure userId is preserved
          name: (document.getElementById('swal-name') as HTMLInputElement).value,
          amount: +(document.getElementById('swal-amount') as HTMLInputElement).value,
          currency: (document.getElementById('swal-currency') as HTMLSelectElement).value,
          billingCycle: +(document.getElementById('swal-cycle') as HTMLSelectElement).value,
          nextPaymentDate: (document.getElementById('swal-date') as HTMLInputElement).value,
          category: (document.getElementById('swal-category') as HTMLSelectElement).value
        };
      }
    });

    if (formValues) {
      this.subscriptionService.updateSubscription(sub.id, formValues).subscribe({
        next: () => {
          Swal.fire('Success', 'Subscription updated!', 'success');
          this.loadSubscriptions();
        },
        error: () => Swal.fire('Error', 'Failed to update subscription', 'error')
      });
    }
  }

  async openAddSubscriptionDialog() {
    const { value: formValues } = await Swal.fire({
      title: 'Add Subscription',
      html: `
        <input id="swal-name" class="swal2-input" placeholder="Name">
        <input id="swal-amount" type="number" class="swal2-input" placeholder="Amount">
        <select id="swal-currency" class="swal2-input">
          <option value="USD" selected>USD</option>
          <option value="EUR">EUR</option>
          <option value="GBP">GBP</option>
          <option value="INR">INR</option>
          <option value="JPY">JPY</option>
          <option value="CAD">CAD</option>
          <option value="AUD">AUD</option>
        </select>
        <select id="swal-cycle" class="swal2-input">
          <option value="1">Monthly</option>
          <option value="2">Yearly</option>
          <option value="0">Weekly</option>
        </select>
        <input id="swal-date" type="date" class="swal2-input" placeholder="Next Payment Date">
        <select id="swal-category" class="swal2-input">
          <option value="" disabled selected>Select Category</option>
          <option value="Entertainment">Entertainment</option>
          <option value="Software">Software</option>
          <option value="Utilities">Utilities</option>
          <option value="Education">Education</option>
          <option value="Productivity">Productivity</option>
          <option value="Other">Other</option>
        </select>
      `,
      focusConfirm: false,
      showCancelButton: true,
      preConfirm: () => {
        return {
          userId: this.userId,
          name: (document.getElementById('swal-name') as HTMLInputElement).value,
          amount: +(document.getElementById('swal-amount') as HTMLInputElement).value,
          currency: (document.getElementById('swal-currency') as HTMLSelectElement).value,
          billingCycle: +(document.getElementById('swal-cycle') as HTMLSelectElement).value,
          nextPaymentDate: (document.getElementById('swal-date') as HTMLInputElement).value,
          category: (document.getElementById('swal-category') as HTMLSelectElement).value
        };
      }
    });

    if (formValues) {
      this.subscriptionService.createSubscription(formValues).subscribe({
        next: () => {
          Swal.fire('Success', 'Subscription added!', 'success');
          this.loadSubscriptions();
        },
        error: () => Swal.fire('Error', 'Failed to add subscription', 'error')
      });
    }
  }

  deleteSubscription(id: string) {
    Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        this.subscriptionService.deleteSubscription(id).subscribe({
          next: () => {
            Swal.fire('Deleted!', 'Subscription has been removed.', 'success');
            this.loadSubscriptions();
          },
          error: () => Swal.fire('Error', 'Failed to delete subscription', 'error')
        });
      }
    });
  }
}
