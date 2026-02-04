export interface LaborTypeRate {
  id?: number;
  laborTypeId: number;
  laborTypeName?: string;

  paymentType?: string;

  dailyRate?: number;
  perTonRate?: number;
  perProductionRate?: number;

  effectiveFrom?: Date;

  isActive: boolean;
}
