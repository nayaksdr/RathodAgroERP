export interface Attendance {
  id: number;
  laborId: number;
  date: string;
  attendanceDate: string;
  isPresent: boolean;
    // 🔹 UI-only field
  laborName?: string;
}
