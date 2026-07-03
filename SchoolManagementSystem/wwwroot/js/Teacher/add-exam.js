$(document).ready(function () {

    // إعادة تهيئة الـ Validation
    function refreshValidation() {
        var form = $("#addExamForm");

        form.removeData("validator");
        form.removeData("unobtrusiveValidation");

        $.validator.unobtrusive.parse(form);
    }

    // تغيير الإجابة الصحيحة
    $(document).on("change", ".correct-radio-trigger", function () {

        var cardBody = $(this).closest(".card-body");
        var currentChoice = $(this).closest(".choice-row").find(".p-2");

        cardBody.find(".choice-row .p-2")
            .removeClass("border-success bg-success bg-opacity-10")
            .addClass("border-light bg-light bg-opacity-50");

        currentChoice
            .removeClass("border-light bg-light bg-opacity-50")
            .addClass("border-success bg-success bg-opacity-10");

        cardBody.find(".correct-option-index-hidden")
            .val($(this).val());
    });

    // حذف سؤال
    $(document).on("click", ".remove-question-btn", function () {

        $(this).closest(".question-card").remove();

        reIndexQuestions();

        refreshValidation();
    });

    // إنشاء سؤال
    function createQuestionMarkup(qIndex) {

        return `
        <div class="card shadow-sm mb-4 border-0 rounded-3 question-card"
             data-q-index="${qIndex}"
             style="border-right:4px solid #7cb1fa !important;">

            <div class="card-header bg-white d-flex justify-content-between align-items-center py-3 border-bottom-0">

                <div class="flex-grow-1 me-3">

                    <span class="badge me-2 q-number-badge"
                          style="background-color:#6da4f1;">
                        سؤال ${qIndex + 1}
                    </span>

                    <input
                        type="text"
                        class="form-control form-control-sm shadow-sm fw-semibold"
                        name="Questions[${qIndex}].QuestionText"
                        placeholder="اكتب نص السؤال هنا..."
                        required />

                </div>

                <div class="d-flex align-items-center">

                    <input
                        type="number"
                        class="form-control form-control-sm text-center shadow-sm me-2"
                        style="width:80px;"
                        name="Questions[${qIndex}].QuestionDegree"
                        value="1"
                        min="1"
                        required />

                    <button
                        type="button"
                        class="btn btn-sm btn-outline-danger remove-question-btn">
                        <i class="bi bi-trash"></i>
                    </button>

                </div>

            </div>

            <div class="card-body p-3 pt-0">

                <input
                    type="hidden"
                    class="correct-option-index-hidden"
                    name="Questions[${qIndex}].Choices.CorrectOptionIndex"
                    value="0" />

                <div class="row g-2 choices-container">

                    ${[0, 1, 2, 3].map(j => `

                        <div class="col-md-6 choice-row">

                            <div class="p-2 rounded border ${j === 0
                ? "border-success bg-success bg-opacity-10"
                : "border-light bg-light bg-opacity-50"}
                                d-flex justify-content-between align-items-center">

                                <input
                                    type="text"
                                    class="form-control form-control-sm border-0 bg-transparent small fw-semibold"
                                    name="Questions[${qIndex}].Choices.Options[${j}]"
                                    placeholder="خيار الإجابة ${j + 1}"
                                    required />

                                <div class="form-check form-check-inline m-0">

                                    <input
                                        type="radio"
                                        class="form-check-input correct-radio-trigger"
                                        name="Questions[${qIndex}].CorrectRadio"
                                        value="${j}"
                                        ${j === 0 ? "checked" : ""} />

                                    <label class="form-check-label small text-muted">
                                        صح
                                    </label>

                                </div>

                            </div>

                        </div>

                    `).join("")}

                </div>

            </div>

        </div>
        `;
    }

    // إضافة سؤال
    $("#addQuestionBtn").on("click", function () {

        var qIndex = $(".question-card").length;

        $("#questionsContainer").append(createQuestionMarkup(qIndex));

        refreshValidation();
    });

    // إنشاء أول سؤال تلقائياً
    $("#questionsContainer").append(createQuestionMarkup(0));

    refreshValidation();

    // إعادة ترقيم الأسئلة
    function reIndexQuestions() {

        $(".question-card").each(function (qIdx) {

            $(this)
                .attr("data-q-index", qIdx);

            $(this)
                .find(".q-number-badge")
                .text("سؤال " + (qIdx + 1));

            $(this)
                .find('input[name$=".QuestionText"]')
                .attr("name", `Questions[${qIdx}].QuestionText`);

            $(this)
                .find('input[name$=".QuestionDegree"]')
                .attr("name", `Questions[${qIdx}].QuestionDegree`);

            $(this)
                .find(".correct-option-index-hidden")
                .attr("name", `Questions[${qIdx}].Choices.CorrectOptionIndex`);

            $(this)
                .find(".correct-radio-trigger")
                .attr("name", `Questions[${qIdx}].CorrectRadio`);

            $(this)
                .find(".choice-row")
                .each(function (cIdx) {

                    $(this)
                        .find('input[type="text"]')
                        .attr(
                            "name",
                            `Questions[${qIdx}].Choices.Options[${cIdx}]`
                        );
                });

        });
    }

});