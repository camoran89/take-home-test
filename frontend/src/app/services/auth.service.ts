import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { tap } from 'rxjs/operators';
import { AuthRequest, AuthResponse } from '../payloads/payloads';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  token = signal('');

  get isAuthenticated(): boolean {
    return !!this.token();
  }

  login(request: AuthRequest) {
    return this.http.post<AuthResponse>(`${environment.apiUrl.replace('/api/loans', '/api/auth/login')}`, request).pipe(
      tap((response) => {
        this.token.set(response.token);
      })
    );
  }

  logout(): void {
    this.token.set('');
  }
}
