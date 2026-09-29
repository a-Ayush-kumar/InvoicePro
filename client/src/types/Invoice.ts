export type InvoiceStatus =
  | "Draft"
  | "Pending"
  | "Paid"
  | "Overdue"
  | "Cancelled";

export type InvoiceResponse = {
  invoiceId: string;
  merchantId: string;
  customerId: string;
  createdByMerchantUserId: string;
  updatedByMerchantUserId: string | null;

  invoiceNumber: string;
  issueDate: string;
  dueDate: string;

  placeOfSupply: string;
  reverseCharge: boolean;

  currency: string;
  subtotal: number;
  discountAmount: number;
  taxableAmount: number;
  totalAmount: number;

  status: InvoiceStatus;
  notes: string;
  termsAndCondition: string;

  createdAt: string;
  updatedAt: string;
};

export type InvoiceListResponse = {
  items: InvoiceResponse[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
};