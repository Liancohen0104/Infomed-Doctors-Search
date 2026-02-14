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

export default function RatingStars({ value = 0, max = 5 }) {
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