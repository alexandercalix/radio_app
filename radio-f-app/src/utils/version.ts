export const isVersionLower = (current: string, min: string) => {
  const a = current.split(".").map(Number);
  const b = min.split(".").map(Number);

  for (let i = 0; i < Math.max(a.length, b.length); i++) {
    if ((a[i] || 0) < (b[i] || 0)) return true;
    if ((a[i] || 0) > (b[i] || 0)) return false;
  }
  return false;
};