/// <reference types="jasmine" />
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { CreateLoanPayload } from '../payloads/payloads';
import { LoanService } from './loan.service';
import { environment } from '../../environments/environment';

describe('LoanService', () => {
  let service: LoanService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [LoanService],
    });

    service = TestBed.inject(LoanService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should request loan list from API', () => {
    const expected = [
      { id: 1, amount: 1000, currentBalance: 1000, applicantName: 'Alice', status: 'active' },
    ];

    service.getLoans().subscribe((loans) => {
      expect(loans).toEqual(expected);
    });

    const req = httpMock.expectOne(environment.apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(expected);
  });

  it('should create loan via API', () => {
    const payload: CreateLoanPayload = {
      amount: 1200,
      currentBalance: 1200,
      applicantName: 'Bob',
    };

    const expected = {
      id: 2,
      amount: 1200,
      currentBalance: 1200,
      applicantName: 'Bob',
      status: 'active',
      paidAt: null,
    };

    service.createLoan(payload).subscribe((loan) => {
      expect(loan).toEqual(expected);
    });

    const req = httpMock.expectOne(environment.apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(payload);
    req.flush(expected);
  });

  it('should post payment to correct endpoint', () => {
    const expected = {
      id: 1,
      amount: 500,
      currentBalance: 400,
      applicantName: 'Alice',
      status: 'active',
      paidAt: null,
    };

    service.payLoan(1, 100).subscribe((loan) => {
      expect(loan).toEqual(expected);
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/1/payment`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ amount: 100 });
    req.flush(expected);
  });
});
