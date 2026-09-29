export type Merchant = {
  merchantId: string;
  adminId: string;

  legalName: string;
  displayName: string;
  email: string;
  phone: string;
  gstin: string | null;

  address: string;
  district: string;
  state: string;
  postalCode: string;
  country: string;

  isActive: boolean;

  createdAt: string;
  updatedAt: string;
};