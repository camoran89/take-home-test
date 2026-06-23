import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Loan } from '../models/models';
import { CreateLoanPayload } from '../payloads/payloads';
import { environment } from '../../environments/environment';

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
