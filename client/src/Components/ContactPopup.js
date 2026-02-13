import { useState } from "react";
import { FiX } from "react-icons/fi";
import "../CSS/ContactPopup.css";

export default function ContactPopup({ doctor, onClose, onSubmit }) {
  const [form, setForm] = useState({
    fullName: "",
    phone: "",
    email: "",
  });

  const [error, setError] = useState("");

  function handleChange(e) {
    setForm({
      ...form,
      [e.target.name]: e.target.value,
    });
  }

  async function handleSubmit(e) {
    e.preventDefault();

    if (!form.fullName || !form.phone || !form.email) {
      setError("נא למלא את כל השדות");
      return;
    }

    setError("");

    try {
      if (onSubmit) {
        await onSubmit({
          ...form,
          doctorId: doctor.id,
          doctorName: doctor.fullName,
        });
      }

      alert("הפנייה נשלחה בהצלחה");
      
      setTimeout(() => {
        onClose();
      }, 2000);

    } catch (err) {
      setError("אירעה שגיאה בשליחה");
    }
  }

  return (
    <div className="popup-overlay" onClick={onClose}>
      <div className="popup" onClick={(e) => e.stopPropagation()}>
        <button className="popup_close" onClick={onClose} type="button">
          <FiX size={20} />
        </button>

        <h3 className="popup_title">
          יצירת קשר עם ד"ר {doctor.fullName}
        </h3>

        <form className="popup_form" onSubmit={handleSubmit}>
          <input
            type="text"
            name="fullName"
            placeholder="שם מלא"
            value={form.fullName}
            onChange={handleChange}
          />

          <input
            type="tel"
            name="phone"
            placeholder="טלפון"
            value={form.phone}
            onChange={handleChange}
          />

          <input
            type="email"
            name="email"
            placeholder="מייל"
            value={form.email}
            onChange={handleChange}
          />

          {error && <div className="popup_error">{error}</div>}

          <button type="submit" className="popup_submit">
            שלח
          </button>
        </form>
      </div>
    </div>
  );
}