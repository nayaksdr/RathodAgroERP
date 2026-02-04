export interface Member {
  id: number;
  name: string;
  balance: number;
}

export interface Expense {
  id: number;
  description: string;
  amount: number;
  paidBy: string;
  date: string;
}

export interface Settlement {
  from: string;
  to: string;
  amount: number;
}

export interface SplitwiseSummaryModel {
  members: Member[];
  expenses: Expense[];
  settlements: Settlement[];
}
