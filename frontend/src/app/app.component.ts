import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { catchError, of } from 'rxjs';
import { Loan } from './loan.model';
import { LoanService } from './loan.service';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './interceptors/auth.interceptor';

@Component({
  selector: 'app-root',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, HttpClientModule],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  loans = signal<Loan[]>([]);
  loading = signal(false);
  error = signal('');
  createApplicant = signal('');
  createAmount = signal(0);
  createBalance = signal(0);
  createError = signal('');
  createLoading = signal(false);
  paymentLoading = signal<number | null>(null);
  loginUsername = signal('');
  loginPassword = signal('');
  loginError = signal('');
  loginLoading = signal(false);
  authService = inject(AuthService);
  loanService = inject(LoanService);
  isAuthenticated = computed(() => this.authService.isAuthenticated);

  constructor() {
    if (this.isAuthenticated()) {
      this.loadLoans();
    }
  }

  login(): void {
    this.loginError.set('');
    const username = this.loginUsername().trim();
    const password = this.loginPassword().trim();

    if (!username || !password) {
      this.loginError.set('Username and password are required.');
      return;
    }

    this.loginLoading.set(true);

    this.authService
      .login({ username, password })
      .pipe(
        catchError(() => {
          this.loginError.set('Login failed. Please verify your credentials.');
          this.loginLoading.set(false);
          return of(null);
        })
      )
      .subscribe((result) => {
        this.loginLoading.set(false);
        if (result) {
          this.loadLoans();
        }
      });
  }

  logout(): void {
    this.authService.logout();
    this.loans.set([]);
    this.loginUsername.set('');
    this.loginPassword.set('');
  }

  loadLoans(): void {
    this.loading.set(true);
    this.error.set('');

    this.loanService
      .getLoans()
      .pipe(
        catchError(() => {
          this.error.set('Unable to load loans. Please check the backend connection.');
          this.loading.set(false);
          return of([] as Loan[]);
        })
      )
      .subscribe((loans) => {
        this.loans.set(loans);
        this.loading.set(false);
      });
  }

  createLoan(): void {
    const applicant = this.createApplicant();
    const amount = this.createAmount();
    const currentBalance = this.createBalance();

    if (!applicant.trim() || amount <= 0 || currentBalance < 0) {
      this.createError.set('Provide a valid applicant name and loan values.');
      return;
    }

    this.createError.set('');
    this.createLoading.set(true);

    this.loanService
      .createLoan({
        applicantName: applicant,
        amount,
        currentBalance,
      })
      .pipe(
        catchError(() => {
          this.createError.set('Unable to create loan. Please try again.');
          this.createLoading.set(false);
          return of(null as Loan | null);
        })
      )
      .subscribe((loan) => {
        if (loan) {
          this.loans.set([...this.loans(), loan]);
          this.createApplicant.set('');
          this.createAmount.set(0);
          this.createBalance.set(0);
        }
        this.createLoading.set(false);
      });
  }

  makePayment(loanId: number, amount: number): void {
    this.paymentLoading.set(loanId);
    this.error.set('');

    this.loanService
      .payLoan(loanId, amount)
      .pipe(
        catchError(() => {
          this.error.set('Unable to process payment.');
          this.paymentLoading.set(null);
          return of(null as Loan | null);
        })
      )
      .subscribe((updatedLoan) => {
        if (updatedLoan) {
          this.loans.set(this.loans().map((loan) => (loan.id === loanId ? updatedLoan : loan)));
        }
        this.paymentLoading.set(null);
      });
  }
}
