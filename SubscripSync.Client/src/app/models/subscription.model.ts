export interface Subscription {
    id: string;
    userId: string;
    name: string;
    amount: number;
    currency: string;
    billingCycle: number; // Enum: 0=Weekly, 1=Monthly, 2=Yearly
    nextPaymentDate: Date;
    category: string;
    isActive: boolean;
}

export enum BillingCycle {
    Weekly = 0,
    Monthly = 1,
    Yearly = 2
}
