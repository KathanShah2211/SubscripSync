import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Subscription } from '../models/subscription.model';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService {
  private apiUrl = 'http://localhost:5083/api/subscriptions'; // Adjust port as needed

  constructor(private http: HttpClient) { }

  getUserSubscriptions(userId: string): Observable<Subscription[]> {
    return this.http.get<Subscription[]>(`${this.apiUrl}/user/${userId}`);
  }
  createSubscription(subscription: any): Observable<string> {
    return this.http.post<string>(this.apiUrl, subscription);
  }

  updateSubscription(id: string, subscription: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, subscription);
  }

  deleteSubscription(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
