export interface CanePurchase {
   id?: number; 
  farmerName?: string;
  date: string;        // ISO date string from API (recommended)
  tons: number;
  rate: number;
  amount: number;
  caneWeightImagePath?: string; // Path to the image
}