
export interface Labor {
  id?: number;
  name: string;
  mobile: string;
    laborTypeId: number | null;   // 👈 IMPORTANT FIX
  laborTypeName?: string; 
  role?: string;
  isActive?: boolean;
}

