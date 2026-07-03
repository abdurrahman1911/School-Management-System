const ctx = document.getElementById('performanceChart');

new Chart(ctx, {
    type: 'line',
    data: {
        labels: ['السبت', 'الأحد', 'الإثنين', 'الثلاثاء', 'الأربعاء', 'الخميس'],
        datasets: [{
            label: 'الأداء',
            data: [60, 70, 75, 85, 90, 95],
            borderColor: '#1e6d92',
            backgroundColor: 'rgba(30,109,146,0.15)',
            tension: 0.4,          // انحناء ناعم
            fill: true,
            pointRadius: 5,
            pointBackgroundColor: '#1e6d92'
        }]
    },
    options: {
        responsive: true,
        plugins: {
            legend: { display: false }
        },
        scales: {
            y: {
                beginAtZero: true,
                max: 100
            }
        }
    }
});