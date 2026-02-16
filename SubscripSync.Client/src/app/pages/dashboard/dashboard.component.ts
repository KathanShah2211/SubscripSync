import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { SubscriptionService } from '../../services/subscription.service';
import { Subscription } from '../../models/subscription.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  subscriptions: Subscription[] = [];
  // Hardcoded User ID for demo
  userId = '3fa85f64-5717-4562-b3fc-2c963f66afa6';

  constructor(private subscriptionService: SubscriptionService) { }

  ngOnInit() {
    this.subscriptionService.getUserSubscriptions(this.userId).subscribe({
      next: (data) => this.subscriptions = data,
      error: (err) => console.error('Error fetching subscriptions', err)
    });
  }
}
