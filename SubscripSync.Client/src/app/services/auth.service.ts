import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'http://localhost:5083/auth';
    private tokenKey = 'auth_token';
    private userKey = 'auth_user';

    constructor(private http: HttpClient, private router: Router) { }

    register(user: any): Observable<any> {
        return this.http.post<any>(`${this.apiUrl}/register`, user).pipe(
            tap(response => this.setSession(response))
        );
    }

    login(credentials: any): Observable<any> {
        return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
            tap(response => this.setSession(response))
        );
    }

    logout(): void {
        localStorage.removeItem(this.tokenKey);
        localStorage.removeItem(this.userKey);
        this.router.navigate(['/login']);
    }

    isLoggedIn(): boolean {
        const token = localStorage.getItem(this.tokenKey);
        // TODO: Add token expiration check
        return !!token;
    }

    getToken(): string | null {
        return localStorage.getItem(this.tokenKey);
    }

    getUser(): any {
        const userStr = localStorage.getItem(this.userKey);
        return userStr ? JSON.parse(userStr) : null;
    }

    getUserId(): string | null {
        const user = this.getUser();
        return user ? user.id : null;
    }

    private setSession(authResult: any): void {
        localStorage.setItem(this.tokenKey, authResult.token);
        // Remove token from user object before storing to avoid duplication/confusion
        const { token, ...user } = authResult;
        localStorage.setItem(this.userKey, JSON.stringify(user));
    }
}
