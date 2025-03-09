const dates = @Html.Raw(Json.Serialize(Model.Stats.Select(s => s.Date.ToString("yyyy-MM-dd"))));
const values = @Html.Raw(Json.Serialize(Model.Stats.Select(s => s.Value)));
console.log("Dates:", dates);
console.log("Values:", values);
const ctx = document.getElementById('myChart');
new Chart(ctx, {
    type: 'line',
    data: {
        labels: dates,
        datasets: [{
            label: 'Giá trị theo ngày',
            data: values,
            borderColor: 'rgba(75, 192, 192, 1)',
            backgroundColor: 'rgba(75, 192, 192, 0.2)',
            borderWidth: 2,
            fill: true
        }]
    },
    options: {
        scales: {
            x: {
                title: { display: true, text: 'Ngày' },
                grid: {
                    display: true,
                    color: 'rgba(0, 0, 0, 0.1)',
                    lineWidth: 1
                }
            },
            y: {
                title: { display: true, text: 'Giá trị' },
                beginAtZero: true,
                grid: {
                    display: true,
                    color: 'rgba(0, 0, 0, 0.1)',
                    lineWidth: 1
                }
            }
        }
    }
});