export interface AdvancePayment {
  id: number;
  laborId: number;
  amount: number;
  dateGiven: string;
  remarks?: string;
  laborName?: string;
  laborType: string;
}
