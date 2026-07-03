// بيانات أداء المعلمين (Mock Data)
const performanceData = {
    mohamed: {
        rate: "جيد جدًا",
        class: "good",
        commitment: 85,
        interaction: 70,
        explanation: 90,
        discipline: 75
    },
    fatma: {
        rate: "ممتاز",
        class: "excellent",
        commitment: 95,
        interaction: 92,
        explanation: 96,
        discipline: 90
    },
    khaled: {
        rate: "جيد",
        class: "good",
        commitment: 70,
        interaction: 65,
        explanation: 80,
        discipline: 72
    },
    mazen: {
        rate: "ضعيف",
        class: "bad",
        commitment: 50,
        interaction: 45,
        explanation: 60,
        discipline: 55
    },
    rawan: {
        rate: "ممتاز",
        class: "excellent",
        commitment: 93,
        interaction: 88,
        explanation: 94,
        discipline: 91
    }
};

// تحديث الأداء
function updatePerformance() {
    const teacher = document.getElementById("teacherSelect").value;

    if (!teacher) return;

    const data = performanceData[teacher];

    // التقييم العام
    const rateEl = document.getElementById("overallRate");
    rateEl.innerText = data.rate;
    rateEl.className = `rate ${data.class}`;

    // تحديث الـ bars
    document.getElementById("commitment").style.height = data.commitment + "%";
    document.getElementById("interaction").style.height = data.interaction + "%";
    document.getElementById("explanation").style.height = data.explanation + "%";
    document.getElementById("discipline").style.height = data.discipline + "%";
}

// تحميل افتراضي
window.onload = function () {
    updatePerformance();
};