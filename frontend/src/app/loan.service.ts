import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Loan } from './loan.model';
import { CreateLoanPayload } from './loan-payload';

@Injectable({
  providedIn: 'root',
})
export class LoanService {
  private http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getLoans(): Observable<Loan[]> {
    return this.http.get<Loan[]>(this.apiUrl);
  }

  createLoan(payload: CreateLoanPayload): Observable<Loan> {
    return this.http.post<Loan>(this.apiUrl, payload);
  }

  payLoan(id: number, amount: number): Observable<Loan> {
    return this.http.post<Loan>(`${this.apiUrl}/${id}/payment`, { amount });
  }
}
