import type { InvoiceResponse, InvoiceListResponse } from "../types/Invoice";

const API_BASE_URL = "http://localhost:5010";

export async function getInvoicesByMerchant(
  merchantId: string,
): Promise<InvoiceListResponse> {
  const response = await fetch(
    `${API_BASE_URL}/api/invoices/merchant/${merchantId}`,
  );

  if (!response.ok) {
    throw new Error(`Failed to fetch invoices: ${response.status}`);
  }

  return response.json();
}