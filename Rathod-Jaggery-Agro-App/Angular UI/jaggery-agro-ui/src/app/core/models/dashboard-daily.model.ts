export interface KeyValueVm {
  date: string;
  value: number;
}

export interface JagerySellVm {
  date: string;
  qty: number;
  amount: number;
}

export interface MemberSummaryVm {
  memberId: number;
  name: string;
  splitwisePaid: number;
  jaggeryEarned: number;
  netBalance: number;
}

export interface DashboardDailyVm {
  todayPresentCount: number;
  todayAdvance: number;
  todayExpense: number;
  todayCaneTons: number;
  todayProduceQty: number;
  todayJaggerySellAmount: number;

  labels: string[];

  attendanceDaily: KeyValueVm[];
  advancesDaily: KeyValueVm[];
  expensesDaily: KeyValueVm[];
  canePurchaseTonsDaily: KeyValueVm[];
  produceQtyDaily: KeyValueVm[];
  jagerySellDaily: JagerySellVm[];

  memberSummary: MemberSummaryVm[];
}
