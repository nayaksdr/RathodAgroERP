export interface CanePaymentSummary {
  id: number;    
  farmerId: number;
  farmerName: string;
  totalPurchase: number;
  totalAdvance: number;
  netAmount: number;
   paymentDate: string;
  paidAmount: number;
  carryForward: number;
  paymentStatus: string;
  isPaid: boolean;
}