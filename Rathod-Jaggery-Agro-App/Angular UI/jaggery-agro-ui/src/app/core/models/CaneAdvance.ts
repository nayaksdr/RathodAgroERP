export interface CaneAdvance {
  id: number;
  farmerId: number;
  memberId: number;
  advanceDate: string;
  amount: number;
  paymentMode: string;
  remarks?: string;
  proofImage?: string;
  farmerName?: string;
  memberName?: string;  
  proofUrl?: string;     // Add this (stores the filename/path from DB)
  

}
