import { useNavigate } from "react-router-dom";
import type { Invoice } from "../types/Invoice";
type Props = {
  invoice: Invoice;
  onClose: () => void;
};

export default function InvoicePreviewModal({ invoice, onClose }: Props) {
  const navigate = useNavigate();
  return (
    <div className="fixed inset-0 z-50 flex justify-center bg-black/50 p-6 backdrop-blur-sm">
      <div className="my-8 flex max-h-[90vh] w-full max-w-5xl flex-col overflow-hidden rounded-3xl border border-border bg-card shadow-2xl">
        {/* Header */}
        <div className="flex items-center justify-between border-b border-border px-8 py-5 shrink-0">
          <div>
            <h2 className="text-2xl font-bold text-foreground">
              Invoice {invoice.id}
            </h2>
            <p className="mt-1 text-sm text-foreground-secondary">
              Preview invoice information
            </p>
          </div>

          <button
            onClick={onClose}
            className="rounded-xl border border-border bg-surface px-4 py-2 text-foreground transition-all duration-300 hover:border-border-hover hover:bg-accent-hover"
          >
            ✕
          </button>
        </div>

        {/* Body */}
        <div className="flex-1 space-y-8 overflow-y-auto p-8">
          {/* Customer & Invoice */}
          <div className="grid grid-cols-2 gap-8">
            <div className="rounded-xl border border-border bg-surface p-5">
              <h3 className="mb-4 text-lg font-semibold text-primary">
                Customer
              </h3>

              <div className="space-y-2 text-sm">
                <p>
                  <span className="font-semibold text-foreground">Name:</span>{" "}
                  John Doe
                </p>
                <p>
                  <span className="font-semibold text-foreground">Email:</span>{" "}
                  john@example.com
                </p>
                <p>
                  <span className="font-semibold text-foreground">Phone:</span>{" "}
                  +91 9876543210
                </p>
                <p>
                  <span className="font-semibold text-foreground">
                    Address:
                  </span>{" "}
                  Delhi, India
                </p>
              </div>
            </div>

            <div className="rounded-xl border border-border bg-surface p-5">
              <h3 className="mb-4 text-lg font-semibold text-primary">
                Invoice
              </h3>

              <div className="space-y-2 text-sm">
                <p>
                  <span className="font-semibold text-foreground">Status:</span>{" "}
                  Paid
                </p>
                <p>
                  <span className="font-semibold text-foreground">
                    Issue Date:
                  </span>{" "}
                  05 Aug 2026
                </p>
                <p>
                  <span className="font-semibold text-foreground">
                    Due Date:
                  </span>{" "}
                  20 Aug 2026
                </p>
                <p>
                  <span className="font-semibold text-foreground">
                    Currency:
                  </span>{" "}
                  EUR (€)
                </p>
              </div>
            </div>
          </div>

          {/* Items */}
          <div className="overflow-hidden rounded-xl border border-border">
            <table className="w-full">
              <thead className="bg-surface">
                <tr className="border-b border-border text-left text-sm text-foreground-secondary">
                  <th className="px-5 py-4">Item</th>
                  <th className="px-5 py-4">Qty</th>
                  <th className="px-5 py-4">Unit Price</th>
                  <th className="px-5 py-4">Total</th>
                </tr>
              </thead>

              <tbody>
                <tr className="border-b border-border hover:bg-accent-hover">
                  <td className="px-5 py-4">Wireless Mouse</td>
                  <td className="px-5 py-4">2</td>
                  <td className="px-5 py-4">€20.00</td>
                  <td className="px-5 py-4">€40.00</td>
                </tr>

                <tr className="hover:bg-accent-hover">
                  <td className="px-5 py-4">Mechanical Keyboard</td>
                  <td className="px-5 py-4">1</td>
                  <td className="px-5 py-4">€80.00</td>
                  <td className="px-5 py-4">€80.00</td>
                </tr>
              </tbody>
            </table>
          </div>

          {/* Summary */}
          <div className="ml-auto w-80 rounded-xl border border-border bg-surface p-5">
            <div className="flex justify-between py-2">
              <span className="text-foreground-secondary">Subtotal</span>
              <span className="font-semibold">€120.00</span>
            </div>

            <div className="flex justify-between py-2">
              <span className="text-foreground-secondary">Tax</span>
              <span className="font-semibold">€24.00</span>
            </div>

            <div className="mt-3 flex justify-between border-t border-border pt-3 text-lg font-bold">
              <span>Total</span>
              <span>€144.00</span>
            </div>
          </div>
        </div>

        {/* Footer */}
        <div className="flex shrink-0 items-center justify-between border-t border-border px-8 py-5">
          <div className="flex gap-3">
            <button
              onClick={() => navigate("/invoice-edit")}
              className="rounded-xl border-b-2 border-border bg-primary px-5 py-2.5 font-semibold text-white transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:bg-primary-hover"
            >
              Edit
            </button>

            <button className="rounded-xl border-b-2 border-border bg-surface px-5 py-2.5 font-medium transition-all duration-300 hover:-translate-y-0.5 hover:bg-accent-hover hover:text-white">
              Download
            </button>

            <button className="rounded-xl border-b-2 border-border bg-surface px-5 py-2.5 font-medium transition-all duration-300 hover:-translate-y-0.5 hover:bg-accent-hover hover:text-white">
              Print
            </button>

            <button className="rounded-xl border-b-2 border-border bg-surface px-5 py-2.5 font-medium transition-all duration-300 hover:-translate-y-0.5 hover:bg-accent-hover hover:text-white">
              Share
            </button>
          </div>

          <div className="flex gap-3">
            <button className="rounded-xl border-b-2 border-border bg-surface px-5 py-2.5 font-medium transition-all duration-300 hover:-translate-y-0.5 hover:bg-accent-hover">
              Archive
            </button>

            <button className="rounded-xl border-b-2 border-danger bg-danger px-5 py-2.5 font-semibold text-white transition-all duration-300 hover:-translate-y-0.5">
              Delete
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
