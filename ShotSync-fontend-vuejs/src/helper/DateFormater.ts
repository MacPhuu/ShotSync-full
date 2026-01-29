export const dateFormater = (dateString: string): string => {
  const date = new Date(dateString);

  // Tạo các mảng chứa tên tháng
  const months = [
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec",
  ];

  // Lấy các giá trị ngày, tháng và năm từ đối tượng Date
  const month = months[date.getUTCMonth()]; // Tháng từ 0-11
  const dayStart = date.getUTCDate(); // Lấy ngày đầu tiên
  const dayEnd = dayStart + 4; // Giả sử thời gian kết thúc sau 4 ngày
  const year = date.getUTCFullYear(); // Năm

  const formattedDate = `${month} ${dayStart} - ${dayEnd}, ${year}`;
  return formattedDate;
};
