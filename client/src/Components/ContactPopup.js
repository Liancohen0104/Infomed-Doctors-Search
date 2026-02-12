import { FiPhoneCall ,FiX } from "react-icons/fi";
import "../CSS/ContactPopup.css";

export default function ContactPopup({ doctor, phone, onClose }) {
  return (
    <div className="popup-overlay" onClick={onClose}>
      <div className="popup" onClick={(e) => e.stopPropagation()}>
        <button className="popup_close" onClick={onClose} type="button">
          <FiX />
        </button>
        <div className="popup_name">ד"ר {doctor.fullName}</div>
        <div className="popup_phone">
          <FiPhoneCall />
          <span>{phone}</span>
        </div>
      </div>
    </div>
  );
}