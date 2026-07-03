



document.addEventListener("DOMContentLoaded", function () {

    const canvas = document.getElementById("successChart");
    const ctx = canvas.getContext("2d");

    let chart = null;

    // بيانات حسب المرحلة + الوقت
    //const data = {
    //    month: {
    //        kg: {
    //            success: [85, 88, 90],
    //            fail: [5, 4, 3]
    //        },
    //        primary: {
    //            success: [80, 82, 85],
    //            fail: [10, 8, 7]
    //        },
    //        prep: {
    //            success: [78, 80, 82],
    //            fail: [12, 10, 9]
    //        },
    //        secondary: {
    //            success: [75, 78, 80],
    //            fail: [15, 12, 10]
    //        }
    //    },

    //    year: {
    //        kg: {
    //            success: [88, 90, 92],
    //            fail: [4, 3, 2]
    //        },
    //        primary: {
    //            success: [85, 87, 90],
    //            fail: [7, 6, 5]
    //        },
    //        prep: {
    //            success: [82, 85, 87],
    //            fail: [9, 7, 6]
    //        },
    //        secondary: {
    //            success: [80, 83, 85],
    //            fail: [10, 8, 7]
    //        }
    //    }
    //};


    const data = data;



    function renderChart() {

        const stage = document.getElementById("stageSelect").value;
        const period = document.getElementById("periodSelect").value;

        const result = data?.[period]?.[stage];

        if (!result) return;

        if (chart) {
            chart.destroy();
        }

        chart = new Chart(ctx, {
            type: "bar",
            data: {
                labels: ["الأول", "الثاني", "الثالث"],
                datasets: [
                    {
                        label: "النجاح",
                        data: result.success,
                        backgroundColor: "#4CAF50",
                        borderRadius: 6
                    },
                    {
                        label: "الرسوب",
                        data: result.fail,
                        backgroundColor: "#e53935",
                        borderRadius: 6
                    }
                ]
            },
            options: {
                responsive: true,
                animation: {
                    duration: 1000,
                    easing: "easeOutQuart"
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
        renderChart();
    }

    document.getElementById("stageSelect").addEventListener("change", updateChart);
    document.getElementById("periodSelect").addEventListener("change", updateChart);

    renderChart();
});