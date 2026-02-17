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
        this.calculateTotal();
      },
      error: (err) => console.error('Error fetching subscriptions', err)
    });
  }

  calculateTotal() {
    this.totalMonthlyCost = 0;
    if (this.subscriptions.length > 0) {
      // Assume first subscription's currency is the preferred one for now
      // In a real app, this would be a user setting or use an exchange rate API
      this.preferredCurrency = this.subscriptions[0].currency;
    }

    this.subscriptions.forEach(sub => {
      let monthlyCost = sub.amount;
      if (sub.billingCycle === 0) monthlyCost *= 4; // Weekly to Monthly
      if (sub.billingCycle === 2) monthlyCost /= 12; // Yearly to Monthly
      this.totalMonthlyCost += monthlyCost;
    });
  }

  async editSubscription(sub: Subscription) {
    const { value: formValues } = await Swal.fire({
      title: 'Edit Subscription',
      html: `
        <input id="swal-name" class="swal2-input" placeholder="Name" value="${sub.name}">
        <input id="swal-amount" type="number" class="swal2-input" placeholder="Amount" value="${sub.amount}">
        <input id="swal-currency" class="swal2-input" placeholder="Currency" value="${sub.currency}">
        <select id="swal-cycle" class="swal2-input">
          <option value="1" ${sub.billingCycle === 1 ? 'selected' : ''}>Monthly</option>
          <option value="2" ${sub.billingCycle === 2 ? 'selected' : ''}>Yearly</option>
          <option value="0" ${sub.billingCycle === 0 ? 'selected' : ''}>Weekly</option>
        </select>
        <input id="swal-date" type="date" class="swal2-input" value="${new Date(sub.nextPaymentDate).toISOString().split('T')[0]}">
        <input id="swal-category" class="swal2-input" placeholder="Category" value="${sub.category}">
      `,
      focusConfirm: false,
      showCancelButton: true,
      preConfirm: () => {
        return {
          id: sub.id,
          userId: this.userId, // Ensure userId is preserved
          name: (document.getElementById('swal-name') as HTMLInputElement).value,
          amount: +(document.getElementById('swal-amount') as HTMLInputElement).value,
          currency: (document.getElementById('swal-currency') as HTMLInputElement).value,
          billingCycle: +(document.getElementById('swal-cycle') as HTMLSelectElement).value,
          nextPaymentDate: (document.getElementById('swal-date') as HTMLInputElement).value,
          category: (document.getElementById('swal-category') as HTMLInputElement).value
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
        <input id="swal-currency" class="swal2-input" placeholder="Currency (USD)" value="USD">
        <select id="swal-cycle" class="swal2-input">
          <option value="1">Monthly</option>
          <option value="2">Yearly</option>
          <option value="0">Weekly</option>
        </select>
        <input id="swal-date" type="date" class="swal2-input" placeholder="Next Payment Date">
        <input id="swal-category" class="swal2-input" placeholder="Category">
      `,
      focusConfirm: false,
      showCancelButton: true,
      preConfirm: () => {
        return {
          userId: this.userId,
          name: (document.getElementById('swal-name') as HTMLInputElement).value,
          amount: +(document.getElementById('swal-amount') as HTMLInputElement).value,
          currency: (document.getElementById('swal-currency') as HTMLInputElement).value,
          billingCycle: +(document.getElementById('swal-cycle') as HTMLSelectElement).value,
          nextPaymentDate: (document.getElementById('swal-date') as HTMLInputElement).value,
          category: (document.getElementById('swal-category') as HTMLInputElement).value
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
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        this.subscriptionService.deleteSubscription(id).subscribe({
          next: () => {
            Swal.fire('Deleted!', 'Your file has been deleted.', 'success');
            this.loadSubscriptions();
          },
          error: () => Swal.fire('Error', 'Failed to delete subscription', 'error')
        });
      }
    });
  }
}
