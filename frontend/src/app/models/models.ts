export interface Loan {
  id: number;
  amount: number;
  currentBalance: number;
  applicantName: string;
  status: string;
  paidAt?: string | null;
}
