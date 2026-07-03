$(document).ready(function () {

    // [1] عند تغيير المرحلة -> جلب الصفوف ديناميكياً
    $('#stageSelect').change(function () {
        var stageId = $(this).val();
        var gradeSelect = $('#gradeSelect');
        gradeSelect.empty().append('<option value="">اختر الصف</option>');
        $('#studentsTable').html('<tr><td colspan="3" style="text-align:center;">يرجى اختيار المرحلة والصف أولاً</td></tr>');

        if (stageId) {
            $.getJSON('/Supervisor/GetGrades', { levelId: stageId }, function (data) {
                $.each(data, function (i, item) {
                    gradeSelect.append($('<option>', { value: item.id, text: item.name }));
                });
            });
        }
    });

    // [2] دالة مشتركة لجلب الطلاب بناءً على الصف والتاريخ الحالي
    function loadStudents() {
        var gradeId = $('#gradeSelect').val();
        var absenceDate = $('#absenceDate').val();

        if (gradeId && absenceDate) {
            $('#studentsTable').html('<tr><td colspan="3" style="text-align:center;"><i class="fas fa-spinner fa-spin"></i> جاري تحميل الطلاب...</td></tr>');

            $.get('/Supervisor/GetStudentList', { gradeId: gradeId, absenceDate: absenceDate }, function (data) {
                $('#studentsTable').html(data);
            });
        } else {
            $('#studentsTable').html('<tr><td colspan="3" style="text-align:center;">يرجى اختيار المرحلة والصف أولاً</td></tr>');
        }
    }

    // استدعاء الدالة عند تغيير الصف أو تغيير التاريخ
    $('#gradeSelect').change(loadStudents);
    $('#absenceDate').change(loadStudents);

    // [3] تفعيل وتعطيل خانة السبب ديناميكياً (حل مشكلة عدم إتاحة الكتابة)
    $('#studentsTable').on('change', '.absent-check', function () {
        var checkbox = $(this);
        var row = checkbox.closest('tr');
        var reasonInput = row.find('.reason-input');

        if (checkbox.is(':checked')) {
            reasonInput.prop('disabled', false).removeAttr('disabled');
            row.addClass('absent-row-selected');
            reasonInput.focus();
        } else {
            reasonInput.prop('disabled', true).attr('disabled', 'disabled').val('');
            row.removeClass('absent-row-selected');
        }
    });

    // [4] حدث الضغط على زر الحفظ (تم إخراجه بشكل مستقل ليعمل دائماً)
    $('#btnSaveAbsence').click(function () {
        var gradeId = $('#gradeSelect').val();
        var absenceDate = $('#absenceDate').val();

        if (!gradeId) {
            alert("يرجى اختيار الصف أولاً");
            return;
        }

        var usersList = [];
        $('.student-row').each(function () {
            var row = $(this);
            var userId = row.find('.user-id').val();
            var isAbsent = row.find('.absent-check').is(':checked');
            var reason = row.find('.reason-input').val();

            usersList.push({
                UserId: parseInt(userId),
                IsAbsent: isAbsent,
                Reason: reason || ""
            });
        });

        var modelData = {
            AbsenceDate: absenceDate,
            Users: usersList
        };

        $('#loadingSpinner').show();
        $('#btnSaveAbsence').prop('disabled', true);

        $.ajax({
            url: '/Supervisor/SaveAbsence',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(modelData),
            success: function (response) {
                $('#loadingSpinner').hide();
                $('#btnSaveAbsence').prop('disabled', false);

                if (response.success) {
                    alert(response.message || "تم حفظ الغياب بنجاح");
                    loadStudents(); // إعادة تحديث الجدول من السيرفر
                } else {
                    alert(response.message || "فشل في حفظ الغياب");
                }
            },
            error: function () {
                $('#loadingSpinner').hide();
                $('#btnSaveAbsence').prop('disabled', false);
                alert("حدث خطأ أثناء الاتصال بالسيرفر.");
            }
        });
    });
});