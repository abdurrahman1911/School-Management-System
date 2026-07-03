
document.addEventListener("DOMContentLoaded", function () {

    const canvas = document.getElementById("attendanceChart");
    const ctx = canvas.getContext("2d");

    let chart = null;

    // بيانات واضحة عشان الفرق يبقى visible
    //const data = {
    //    week: {
    //        kg: { present: 60, absent: 40 },
    //        primary: { present: 70, absent: 30 },
    //        prep: { present: 80, absent: 20 },
    //        secondary: { present: 75, absent: 25 }
    //    },
    //    month: {
    //        kg: { present: 90, absent: 10 },
    //        primary: { present: 95, absent: 5 },
    //        prep: { present: 88, absent: 12 },
    //        secondary: { present: 85, absent: 15 }
    //    }
    //};

    const data = attendanceData;

    function animateUpdate(newData) {

        if (chart) {
            chart.destroy();
        }

        chart = new Chart(ctx, {
            type: "bar",
            data: {
                labels: ["النسبة"],
                datasets: [
                    {
                        label: "نسبة الحضور",
                        data: [newData.present],
                        backgroundColor: "#4CAF50",
                        borderRadius: 6
                    },
                    {
                        label: "نسبة الغياب",
                        data: [newData.absent],
                        backgroundColor: "#e53935",
                        borderRadius: 6
                    }
                ]
            },
            options: {
                responsive: true,
                animation: {
                    duration: 1200,
                    easing: "easeOutQuart"
                },
                plugins: {
                    legend: {
                        display: true
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        max: 100,
                        ticks: {
                            stepSize: 10
                        }
                    }
                }
            }
        });
    }

    function updateChart() {

        const stage = document.getElementById("stageSelect").value;
        const period = document.getElementById("periodSelect").value;

        const result = data?.[period]?.[stage];

        console.log("Selected:", stage, period, result);

        if (!result) return;

        animateUpdate(result);
    }

    // Events
    document.getElementById("stageSelect").addEventListener("change", updateChart);
    document.getElementById("periodSelect").addEventListener("change", updateChart);

    // أول تحميل
    updateChart();
});