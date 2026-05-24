import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError, map, Observable, of, tap } from 'rxjs';

import { LoginResponse } from '../models/loginResponse.model';
import { Workspace } from '../models/workspace.model';
import { LoginRequest } from '../models/loginRequest.model';
import { isPlatformBrowser } from '@angular/common';
import { environment } from '../../../environments/environment';
import { TokenRequest } from '../models/tokenRequest.model';
import { jwtDecode } from 'jwt-decode';
import { JwtPayload } from '../models/Jwtpayload.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly apiUrl = environment.apiUrl;
  private readonly apiRoute = 'auth';
  private readonly isAuthenticatedSubject = new BehaviorSubject<boolean>(this.checkToken());
  readonly isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  private readonly availableLoginSubject = new BehaviorSubject<LoginResponse | null>(null);
  readonly availableWorkspaces$ = this.availableLoginSubject.asObservable();


  login(credentials: LoginRequest): Observable<boolean> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/${this.apiRoute}/login`, credentials).pipe(
      tap((response) => {
        this.availableLoginSubject.next(response);
      }),
      map(() => true),
      catchError(() => of(false)),
    );
  }

  selectWorkspace(tokenRequest: TokenRequest): Observable<boolean> {
    return this.http
      .post<{ token: string }>(`${this.apiUrl}/${this.apiRoute}/token`, tokenRequest)
      .pipe(
        tap((response) => {
          this.saveToken(response.token);
          this.isAuthenticatedSubject.next(true);
        }),
        map(() => true),
        catchError(() => of(false)),
      );
  }

  private checkToken(): boolean {
    if (isPlatformBrowser(this.platformId)) {
      return !!localStorage.getItem('auth_token');
    }
    return false;
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    this.isAuthenticatedSubject.next(false);
    this.availableLoginSubject.next(null);
  }

  get isLoggedIn(): boolean {
    return this.isAuthenticatedSubject.value;
  }

  get getUser(): User | null {
    return this.availableLoginSubject.value?.user || null;
  }

  get getWorkspaces(): Workspace[] {
    return this.availableLoginSubject.value?.workspaces || [];
  }

  getTokenInfo(): JwtPayload | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }

    const token = localStorage.getItem('auth_token');
    if (!token) {
      return null;
    }

    try {
      const decode = jwtDecode<JwtPayload>(token);
      return decode;
    } catch (error) {
      console.error('Error al decodificar el token:', error);
      return null;
    }
  }

  private saveToken(token: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('auth_token', token);
    }
  }

}
