let shift = 0;

renderSchedule();

document.getElementById("nextmonth").addEventListener("click", function () {
    shift++;
    renderSchedule(shift);
});

document.getElementById("prevmonth").addEventListener("click", function () {
    shift--;
    renderSchedule(shift);
});

document.getElementById("today").addEventListener("click", function () {
    shift = 0;
    renderSchedule(shift);
});

function renderSchedule(shift = 0) {
    let dates = new Array();
    let today = new Date();

    today.setMonth(today.getMonth() + shift);

    let month = today.getMonth();
    let year = today.getFullYear();
    let firstDayInMonth = new Date(year, month);

    document.getElementById("month").innerHTML = today.toLocaleString('default', { month: 'long' });
    document.getElementById("year").innerHTML = year;

    while (firstDayInMonth.getDay() != 1) {
        firstDayInMonth.setDate(firstDayInMonth.getDate() - 1);
    }


    while (dates.length < 42) {
        dates.push(new Date(firstDayInMonth.getFullYear(), firstDayInMonth.getMonth(), firstDayInMonth.getDate()));
        firstDayInMonth.setDate(firstDayInMonth.getDate() + 1);
    }

    let scheduleTable = document.getElementById("scheduleTable");
    scheduleTable.innerHTML = "";
    let schedule = getSchedule();
    for (const element of dates) {
        let day = document.createElement("div");
        day.setAttribute("month", element.getMonth());
        day.setAttribute("day", element.getDate());
        day.innerHTML = "<div>" + element.getDate() + "</div>";
        if (schedule != undefined) {
            if (schedule[element.getDay()].StartTime != "" && schedule[element.getDay()].EndTime != "") {
                day.innerHTML += "<p>" + schedule[element.getDay()].StartTime + ' - ' + schedule[element.getDay()].EndTime + "</p>";
            }
        }

        if (element.getMonth() != month) {
            day.classList.add("otherMonth");
        }
        else {
            day.classList.add("thisMonth");
        }

        if (element.getDate() == new Date().getDate() && element.getMonth() == new Date().getMonth() && element.getFullYear() == new Date().getFullYear()) {
            day.classList.add("today");
        }

        scheduleTable.appendChild(day);
    }
}

function getSchedule() {
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "/Account/Schedule", false);
    xhr.setRequestHeader('Content-Type', 'x-www-form-urlencoded');
    xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
    xhr.send();
    if (xhr.status === 200) {
        let schedule = JSON.parse(xhr.responseText);
        if (schedule != null) {
            return JSON.parse(schedule.ScheduleJson);
        }
    }
    else {
        return Object();
    }
}
