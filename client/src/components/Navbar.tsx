"use client";

import { Link } from "react-router-dom";

export default function Navbar() {
  return (
    <nav className="sticky top-0 z-50 bg-surface rounded-2xl border-b-4 border-border hover:border-border-hover">
      <div className="mx-auto flex h-18 max-w-7xl items-center justify-between px-8">
        <Link
          to="/"
          className="text-2xl font-bold tracking-tight text-primary hover:text-primary-hover transform hover:scale-[1.05] duration-1000"
        >
          Tenderlink Services
        </Link>

        <div className="flex items-center gap-4">
          <button className="rounded-xl bg-surface border-b-2 border-border px-5 py-2.5 text-sm font-semibold transition-all duration-1000 hover:-translate-y-0.5 transform hover:scale-[1.05] hover:bg-accent-hover">
            Login
          </button>

          <button className="rounded-xl bg-surface border-b-2 border-border px-5 py-2.5 text-sm font-semibold transition-all duration-1000 hover:-translate-y-0.5 transform hover:scale-[1.05] hover:bg-danger">
            Logout
          </button>
        </div>
      </div>
    </nav>
  );
}
