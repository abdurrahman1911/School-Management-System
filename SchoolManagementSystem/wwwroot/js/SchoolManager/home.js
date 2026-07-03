
// بيانات الجراف
const chartData = {
    supervisorsChart: [70, 75, 65, 50, 30],

    teachersChart: [60, 70, 65, 90, 40],

    studentsChart: [55, 65, 60, 88, 45]
};

// ألوان
const colors = ['#ff6b6b', '#f7b32b', '#6a2c70', '#1abc9c', '#3498db'];

// رسم الجراف
// أسماء التقييمات
const labels = ['ممتاز', 'جيد جدًا', 'جيد', 'مقبول', 'ضعيف'];
// رسم الجراف
function renderChart(chartId) {
    const chartEl = document.getElementById(chartId);
    chartEl.innerHTML = '';

const data = chartData[chartId];
    data.forEach((value, i) => {

        // container
        const item = document.createElement('div');
        item.classList.add('chart-item');

        // العمود
        const bar = document.createElement('div');
        bar.classList.add('bar');
        bar.style.background = colors[i % colors.length];
        bar.style.height = '0%';

        // النص
        const label = document.createElement('span');
        label.classList.add('bar-label');
        label.innerText = labels[i];

        item.appendChild(bar);
        item.appendChild(label);

        chartEl.appendChild(item);

        setTimeout(() => {
            bar.style.height = value + '%';
        }, 100);
    });
}


window.onload = function () {
    renderChart('supervisorsChart');
    renderChart('teachersChart');
    renderChart('studentsChart');
};