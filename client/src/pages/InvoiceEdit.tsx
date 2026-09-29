import { Link } from "react-router-dom";

export default function InvoiceEdit() {
  return (
    <main className="mx-auto max-w-7xl px-6 py-8">
      {/* Header */}
      <div className="mb-8 flex flex-wrap items-center justify-between gap-4">
        <div>
          <Link
            to="/"
            className="mb-2 inline-flex text-sm text-primary transition-all duration-300 hover:text-primary-hover"
          >
            ← Back to Dashboard
          </Link>

          <h1 className="text-3xl font-bold text-foreground">Edit Invoice</h1>

          <p className="mt-2 text-foreground-secondary">INV-2026-001</p>
        </div>

        <div className="flex gap-3">
          <button className="rounded-xl border-b-2 border-border bg-surface px-4 py-2 text-sm font-semibold text-primary transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:border-border-hover hover:bg-accent-hover hover:text-primary-hover">
            Cancel
          </button>

          <button className="rounded-xl border-b-2 border-border bg-surface px-4 py-2 text-sm font-semibold text-primary transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:border-border-hover hover:bg-accent-hover hover:text-primary-hover">
            Save Draft
          </button>

          <button className="rounded-xl border-b-2 border-border bg-primary px-5 py-2 text-sm font-semibold text-white transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:bg-primary-hover">
            Save Invoice
          </button>
        </div>
      </div>

      {/* Company & Customer */}

      <div className="mb-8 grid gap-6 lg:grid-cols-2">
        <section className="rounded-2xl border border-border bg-card p-6">
          <h2 className="mb-5 text-lg font-semibold text-primary">
            Company Information
          </h2>

          <div className="space-y-4">
            <input
              placeholder="Company Name"
              className="w-full rounded-xl border border-border bg-surface px-4 py-3 outline-none focus:border-primary"
            />

            <input
              placeholder="VAT Number"
              className="w-full rounded-xl border border-border bg-surface px-4 py-3 outline-none focus:border-primary"
            />

            <textarea
              rows={4}
              placeholder="Company Address"
              className="w-full rounded-xl border border-border bg-surface px-4 py-3 outline-none resize-none focus:border-primary"
            />
          </div>
        </section>

        <section className="rounded-2xl border border-border bg-card p-6">
          <h2 className="mb-5 text-lg font-semibold text-primary">
            Customer Information
          </h2>

          <div className="space-y-4">
            <input
              placeholder="Customer Name"
              className="w-full rounded-xl border border-border bg-surface px-4 py-3 outline-none focus:border-primary"
            />

            <input
              placeholder="Email"
              className="w-full rounded-xl border border-border bg-surface px-4 py-3 outline-none focus:border-primary"
            />

            <input
              placeholder="Phone Number"
              className="w-full rounded-xl border border-border bg-surface px-4 py-3 outline-none focus:border-primary"
            />

            <textarea
              rows={4}
              placeholder="Billing Address"
              className="w-full rounded-xl border border-border bg-surface px-4 py-3 outline-none resize-none focus:border-primary"
            />
          </div>
        </section>
      </div>

      {/* Invoice Details */}

      <section className="mb-8 rounded-2xl border border-border bg-card p-6">
        <h2 className="mb-6 text-lg font-semibold text-primary">
          Invoice Details
        </h2>

        <div className="grid gap-5 md:grid-cols-2 xl:grid-cols-4">
          <input
            placeholder="Invoice Number"
            className="rounded-xl border border-border bg-surface px-4 py-3 outline-none focus:border-primary"
          />

          <input
            type="date"
            className="rounded-xl border border-border bg-surface px-4 py-3 outline-none focus:border-primary"
          />

          <input
            type="date"
            className="rounded-xl border border-border bg-surface px-4 py-3 outline-none focus:border-primary"
          />

          <select className="rounded-xl border border-border bg-surface px-4 py-3 outline-none focus:border-primary">
            <option>Draft</option>
            <option>Pending</option>
            <option>Paid</option>
          </select>
        </div>
      </section>

      {/* Items */}

      <section className="mb-8 rounded-2xl border border-border bg-card p-6">
        <div className="mb-5 flex items-center justify-between">
          <h2 className="text-lg font-semibold text-primary">Invoice Items</h2>

          <button className="rounded-xl border-b-2 border-border bg-surface px-4 py-2 text-sm font-semibold text-primary transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:border-border-hover hover:bg-surface-hover hover:text-primary-hover">
            + Add Item
          </button>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full">
            <thead>
              <tr className="border-b border-border text-left text-sm text-foreground-secondary">
                <th className="py-3">Description</th>
                <th>Qty</th>
                <th>Unit Price</th>
                <th>Tax</th>
                <th>Total</th>
                <th></th>
              </tr>
            </thead>

            <tbody>
              {[1, 2].map((item) => (
                <tr key={item} className="border-b border-border">
                  <td className="py-4">
                    <input
                      placeholder="Item description"
                      className="w-full rounded-lg border border-border bg-surface px-3 py-2 outline-none"
                    />
                  </td>

                  <td>
                    <input className="w-20 rounded-lg border border-border bg-surface px-3 py-2 outline-none" />
                  </td>

                  <td>
                    <input className="w-28 rounded-lg border border-border bg-surface px-3 py-2 outline-none" />
                  </td>

                  <td>
                    <input className="w-20 rounded-lg border border-border bg-surface px-3 py-2 outline-none" />
                  </td>

                  <td className="font-semibold text-foreground">€0.00</td>

                  <td>
                    <button className="rounded-xl border-b-2 border-border bg-surface px-3 py-2 text-sm font-semibold text-danger transition-all duration-500 hover:-translate-y-0.5 hover:scale-[1.05] hover:border-border-hover hover:bg-surface-hover">
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>

      {/* Notes & Summary */}

      <div className="grid gap-6 lg:grid-cols-2">
        <section className="rounded-2xl border border-border bg-card p-6">
          <h2 className="mb-5 text-lg font-semibold text-primary">Notes</h2>

          <textarea
            rows={8}
            placeholder="Additional Notes..."
            className="w-full rounded-xl border border-border bg-surface px-4 py-3 outline-none resize-none focus:border-primary"
          />
        </section>

        <section className="rounded-2xl border border-border bg-card p-6">
          <h2 className="mb-5 text-lg font-semibold text-primary">Summary</h2>

          <div className="space-y-4">
            <div className="flex justify-between">
              <span className="text-foreground-secondary">Subtotal</span>
              <span>€120.00</span>
            </div>

            <div className="flex justify-between">
              <span className="text-foreground-secondary">Tax</span>
              <span>€24.00</span>
            </div>

            <div className="flex justify-between">
              <span className="text-foreground-secondary">Discount</span>
              <span>€0.00</span>
            </div>

            <div className="mt-5 border-t border-border pt-5 flex justify-between text-xl font-bold">
              <span>Total</span>
              <span>€144.00</span>
            </div>
          </div>
        </section>
      </div>
    </main>
  );
}
