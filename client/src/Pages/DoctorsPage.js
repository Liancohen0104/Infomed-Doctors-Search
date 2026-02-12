import { useEffect, useState } from "react";
import { fetchDoctors, fetchActiveDoctors, fetchPromotedDoctors } from "../Api/DoctorApi";
import { FiPhoneCall, FiSend, FiSearch } from "react-icons/fi";
import ContactPopup from "../Components/ContactPopup";
import "../CSS/DoctorsPage.css";

function formatPhone(raw = "") {
  const digits = raw.replace(/\D/g, "");
  if (/^05\d/.test(digits) || /^07\d/.test(digits)) {
    if (digits.length >= 3 && digits[3] !== "-") {
      return digits.slice(0, 3) + "-" + digits.slice(3);
    }
  } else if (/^0\d/.test(digits)) {
    if (digits.length >= 2 && digits[2] !== "-") {
      return digits.slice(0, 2) + "-" + digits.slice(2);
    }
  }
  return raw;
}

function StarFull() {
  return (
    <svg width="15" height="15" viewBox="0 0 24 24">
      <polygon
        points="12,2 15.09,8.26 22,9.27 17,14.14 18.18,21.02 12,17.77 5.82,21.02 7,14.14 2,9.27 8.91,8.26"
        fill="#3dbfbf"
      />
    </svg>
  );
}

function StarEmpty() {
  return (
    <svg width="15" height="15" viewBox="0 0 24 24">
      <polygon
        points="12,2 15.09,8.26 22,9.27 17,14.14 18.18,21.02 12,17.77 5.82,21.02 7,14.14 2,9.27 8.91,8.26"
        fill="none"
        stroke="#3dbfbf"
        strokeWidth="1.5"
      />
    </svg>
  );
}

function RatingStars({ value = 0, max = 5 }) {
  const rating = Math.max(0, Math.min(max, Number(value) || 0));
  const fullCount = Math.round(rating);
  return (
    <div className="rating">
      <div className="rating_stars">
        {Array.from({ length: max }).map((_, i) =>
          i < fullCount ? <StarFull key={i} /> : <StarEmpty key={i} />
        )}
      </div>
      <span className="rating_value">({rating.toFixed(1)})</span>
    </div>
  );
}

export default function DoctorsPage() {
  const [mode, setMode] = useState("all");
  const [doctors, setDoctors] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [search, setSearch] = useState("");
  const [popupDoctor, setPopupDoctor] = useState(null);

  function toggleMode(nextMode) {
    setMode((prev) => (prev === nextMode ? "all" : nextMode));
  }

  useEffect(() => {
    let isMounted = true;
    async function load() {
      setLoading(true);
      setError("");
      try {
        let data;
        if (mode === "active") data = await fetchActiveDoctors();
        else if (mode === "promoted") data = await fetchPromotedDoctors();
        else data = await fetchDoctors();
        if (isMounted) setDoctors(data);
      } catch {
        if (isMounted)
          setError("שגיאה בטעינת הרופאים. ודאי שהשרת רץ על http://localhost:5153 ושאין בעיית CORS.");
      } finally {
        if (isMounted) setLoading(false);
      }
    }
    load();
    return () => { isMounted = false; };
  }, [mode]);

  const filtered = doctors.filter((d) =>
    d.fullName?.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="doctors-page">
      <div className="doctors-page_logoWrapper">
        <img
          src="/infomed-logo.png"
          alt="Infomed"
          className="doctors-page_logo"
        />
      </div>

      <div className="doctors-page_search">
        <FiSearch className="search_icon" />
        <input
          className="search_input"
          type="text"
          placeholder="חיפוש לפי שם רופא..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>

      <div className="doctors-page_filters">
        <button
          className={`toggle-btn ${mode === "active" ? "toggle-btn--active" : ""}`}
          onClick={() => toggleMode("active")}
          type="button"
        >
          הצג רק רופאים פעילים
        </button>
        <button
          className={`toggle-btn ${mode === "promoted" ? "toggle-btn--active" : ""}`}
          onClick={() => toggleMode("promoted")}
          type="button"
        >
          הצג רק רופאים משלמים
        </button>
      </div>

      {loading && <div className="doctors-page_status">טוען רופאים...</div>}
      {error && <div className="doctors-page_error">{error}</div>}

      {!loading && !error && (
        <div className="doctors-page_list">
          {filtered.length === 0 ? (
            <div className="doctors-page_status">אין תוצאות.</div>
          ) : (
            filtered.map((d) => {
              const phone = formatPhone(d?.phones?.[0]?.number ?? "");
              const languages = d?.languageIds?.length ? d.languageIds.join(", ") : "";

              return (
                <div className="doctorRow" key={d.id}>
                  <div className="doctorRow_right">
                    <div className="doctorRow_name">ד"ר {d.fullName}</div>
                    <RatingStars value={d?.reviews?.averageRating ?? 0} />
                  </div>

                  <div className="doctorRow_middle">
                    <div className="doctorRow_line">
                      <span className="doctorRow_label">נותן שירות בשפה: </span>
                      <span>{languages}</span>
                    </div>
                  </div>

                  <div className="doctorRow_left">
                    <button className="doctorRow_phoneBtn" type="button">
                      <span>{phone}</span>
                      <FiPhoneCall />
                    </button>
                    <button
                      className="doctorRow_contactBtn"
                      type="button"
                      onClick={() => setPopupDoctor({ doctor: d, phone })}
                    >
                      <span>צור קשר</span>
                      <FiSend />
                    </button>
                  </div>
                </div>
              );
            })
          )}
        </div>
      )}

      {popupDoctor && (
        <ContactPopup
          doctor={popupDoctor.doctor}
          phone={popupDoctor.phone}
          onClose={() => setPopupDoctor(null)}
        />
      )}
    </div>
  );
}
