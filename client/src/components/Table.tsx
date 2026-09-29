"use client";
import { useEffect, useState } from "react";
import { getInvoicesByMerchant } from "../api/invoiceApi";
import type { InvoiceResponse } from "../types/Invoice";

type TableProps = {
  onView: (invoice: InvoiceResponse) => void;
};

const MERCHANT_ID = "6dcd9679-b279-4f58-919b-8a6ea5337673";

// const badgeColor = (status: string) => {
//   switch (status) {
//     case "Paid":
//       return "bg-green-50 text-green-700";
//     case "Pending":
//       return "bg-yellow-50 text-yellow-700";
//     case "Draft":
//       return "bg-secondary-100 text-secondary-700";
//     case "Overdue":
//       return "bg-red-50 text-red-700";
//     default:
//       return "";
//   }
// };

export default function Table({ onView }: TableProps) {
  const [invoices, setInvoices] = useState<InvoiceResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function loadInvoices() {
      try {
        setLoading(true);
        setError(null);

        const response = await getInvoicesByMerchant(MERCHANT_ID);

        setInvoices(response.items);
      } catch (error) {
        setError(
          error instanceof Error ? error.message : "Failed to load invoices",
        );
      } finally {
        setLoading(false);
      }
    }

    loadInvoices();
  }, []);

  if (loading) {
    return (
      <div className="rounded-2xl border border-border bg-card p-8 text-center text-foreground-secondary">
        Loading invoices...
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-2xl border border-danger bg-card p-8 text-center text-danger">
        {error}
      </div>
    );
  }

  return (
    <div className="overflow-hidden rounded-2xl border border-border bg-card">
      <div className="overflow-x-auto">
        <table className="min-w-full divide-y divide-border">
          <thead className="bg-surface">
            <tr>
              <th className="px-6 py-4 text-left text-sm font-semibold text-foreground">
                Invoice
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-foreground">
                Customer
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-foreground">
                Issue Date
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-foreground">
                Due Date
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-foreground">
                Status
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-foreground">
                Amount
              </th>
              <th className="px-6 py-4 text-right text-sm font-semibold text-foreground">
                Action
              </th>
            </tr>
          </thead>

          <tbody className="divide-y divide-border">
            {invoices.map((invoice) => (
              <tr
                key={invoice.invoiceId}
                className="hover:bg-surface-hover"
              >
                <td className="px-6 py-4 text-sm font-medium text-foreground">
                  {invoice.invoiceNumber}
                </td>

                <td className="px-6 py-4 text-sm text-foreground-secondary">
                  {invoice.customerId}
                </td>

                <td className="px-6 py-4 text-sm text-foreground-secondary">
                  {new Date(invoice.issueDate).toLocaleDateString()}
                </td>

                <td className="px-6 py-4 text-sm text-foreground-secondary">
                  {new Date(invoice.dueDate).toLocaleDateString()}
                </td>

                <td className="px-6 py-4">
                  <span className="rounded-full px-3 py-1 text-xs font-medium">
                    {invoice.status}
                  </span>
                </td>

                <td className="px-6 py-4 text-sm font-medium text-foreground">
                  {invoice.currency} {invoice.totalAmount.toFixed(2)}
                </td>

                <td className="px-6 py-4 text-right">
                  <button
                    onClick={() => onView(invoice)}
                    className="font-medium text-primary hover:text-primary-hover"
                  >
                    View
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="border-t border-border px-6 py-4 text-sm text-foreground-secondary">
        Showing {invoices.length} of {invoices.length} invoices
      </div>
    </div>
  );
}