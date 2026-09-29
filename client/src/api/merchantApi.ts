import type { Merchant } from "../types/Merchant";

const API_BASE_URL = "https://localhost:5010";

export async function getMerchantById(
  merchantId: string,
): Promise<Merchant> {
  const response = await fetch(
    `${API_BASE_URL}/api/Merchants/${merchantId}`,
  );

  if (!response.ok) {
    throw new Error(`Failed to fetch merchant: ${response.status}`);
  }

  return response.json();
}