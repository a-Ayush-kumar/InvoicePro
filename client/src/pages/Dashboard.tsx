import InvoicePreviewModal from "../components/InvoicePreviewModal";
import Table from "../components/Table";
import { useState, useEffect } from "react";
import { getMerchantById } from "../api/merchantApi";
import type { Merchant } from "../types/Merchant";
import type { InvoiceResponse } from "../types/Invoice";

export default function Dashboard() {
  const [merchant, setMerchant] = useState<Merchant | null>(null);
  const [merchantError, setMerchantError] = useState<string | null>(null);
  const [selectedInvoice, setSelectedInvoice] =
    useState<InvoiceResponse | null>(null);

  useEffect(() => {
    async function loadMerchant() {
      try {
        const data = await getMerchantById(
          "6dcd9679-b279-4f58-919b-8a6ea5337673",
        );

        setMerchant(data);
      } catch (error) {
        setMerchantError(
          error instanceof Error ? error.message : "Failed to load merchant",
        );
      }
    }

    loadMerchant();
  }, []);

  return (
    <main className="mx-auto max-w-7xl px-6 py-10 lg:px-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold tracking-tight text-foreground">
          {merchant?.displayName ?? "Dashboard"}
          {merchantError && <p className="mt-2 text-danger">{merchantError}</p>}
        </h1>

        <p className="mt-2 text-foreground-secondary">
          Manage and track all company invoices.
        </p>
      </div>

      <div className="mb-6 flex flex-wrap items-start justify-between gap-4">
        <button
          className="
      rounded-xl border-b-2 border-border bg-primary px-5 py-3 font-semibold text-surface transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:bg-primary-hover hover:border-border-hover"
        >
          + New Invoice
        </button>

        <div className="flex flex-col items-end gap-4">
          <input
            type="text"
            placeholder="Search invoices..."
            className="w-80 rounded-xl border-b-2 border-border bg-surface px-4 py-3 text-foreground placeholder:text-foreground-muted outline-none transition-all duration-500 hover:border-border-hover focus:border-primary focus:bg-card"
          />

          <div className="flex flex-wrap justify-end gap-3">
            <button className="rounded-xl border-b-2 border-border bg-surface px-4 py-2 text-sm font-semibold text-primary transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:border-border-hover hover:bg-accent-hover hover:text-primary-hover">
              Filter
            </button>
            <button className="rounded-xl border-b-2 border-border bg-surface px-4 py-2 text-sm font-semibold text-primary transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:border-border-hover hover:bg-accent-hover hover:text-primary-hover">
              Sort
            </button>
            <button className="rounded-xl border-b-2 border-border bg-surface px-4 py-2 text-sm font-semibold text-primary transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:border-border-hover hover:bg-accent-hover hover:text-primary-hover">
              Import
            </button>
            <button className="rounded-xl border-b-2 border-border bg-surface px-4 py-2 text-sm font-semibold text-primary transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:border-border-hover hover:bg-accent-hover hover:text-primary-hover">
              Export
            </button>
            <button className="rounded-xl border-b-2 border-border bg-surface px-4 py-2 text-sm font-semibold text-primary transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:border-border-hover hover:bg-accent-hover hover:text-primary-hover">
              Refresh
            </button>
          </div>
        </div>
      </div>

      <Table onView={setSelectedInvoice} />
      {/* Modal */}
      {selectedInvoice && (
        <InvoicePreviewModal
          invoice={selectedInvoice}
          onClose={() => setSelectedInvoice(null)}
        />
      )}
    </main>
  );
}
