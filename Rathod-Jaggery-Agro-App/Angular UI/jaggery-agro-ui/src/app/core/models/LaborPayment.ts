export interface LaborPayment {
  laborId: string;
  laborName: string;
  fromDate: string;
  toDate: string;
  workAmount: number;   // Days, Tons, or Units
  rate: number;
  grossAmount: number;  // Matches C# GrossAmount
  advanceAdjusted: number; // Matches C# AdvanceAdjusted
  netAmount: number;    // Matches C# NetAmount
  isPaid: boolean;
  laborTypeName: string;
}