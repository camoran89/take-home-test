/// <reference types="jasmine" />
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { LoanService } from '../../services/loan.service';
import { of } from 'rxjs';
import { Loan } from '../../models/models';

describe('AppComponent', () => {
  let fixture: ComponentFixture<AppComponent>;
  let component: AppComponent;
  let loanService: jasmine.SpyObj<LoanService>;

  const loans: Loan[] = [
    { id: 1, amount: 1000, currentBalance: 1000, applicantName: 'Alice', status: 'active' },
  ];

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('LoanService', ['getLoans', 'createLoan', 'payLoan']);
    spy.getLoans.and.returnValue(of(loans));

    await TestBed.configureTestingModule({
      imports: [AppComponent, HttpClientTestingModule],
      providers: [{ provide: LoanService, useValue: spy }],
    }).compileComponents();

    loanService = TestBed.inject(LoanService) as jasmine.SpyObj<LoanService>;
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should load loans on init', () => {
    expect(component.loans()).toEqual(loans);
  });

  it('should call getLoans once', () => {
    expect(loanService.getLoans).toHaveBeenCalledTimes(1);
  });
});
