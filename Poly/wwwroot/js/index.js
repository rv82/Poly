const svg = SVG().addTo("#drawing-block").size(640, 480);

// радиус точки (фактически круга) на экране
const radius = 3;

// Признак задания координат многоугольника.
// Если false, то включен режим задания вершин многоугольника.
// Если true, то включен режим задания контрольных точек.
let isDone = false;

// Массив вершин многоугольника
let points = [];

// используется для задания имени точки (1, 2, 3, ...)
let pointIndex = 0;

/**
 * @typedef {Object} Result
 * @property {String} name - название точки
 * @property {Boolean} inPolygon - признак, показывающий, лежит ли точка внутри многоугольника или вне его.
 */
/**
 * Функция добавляет на страницу строку вида "точка 1 лежит внутри многоугольника"
 * @param {Result} result - результат от backend'а.
 */
function addResultLine(result) {
    let resultsBlock = document.getElementById("results");
    let row = document.createElement("div");
    row.innerHTML = `точка ${result.name} лежит ${result.inPolygon
            ? '<span class="in-poly">внутри</span>'
            : '<span class="out-poly">вне</span>'
        } многоугольника`;
    resultsBlock.appendChild(row);
}

/**
 * @typedef {Object} Point - координата точки. Соответствует типу Point в C#-коде
 * @property {Number} x - абсцисса точки
 * @property {Number} y - ордината точки
 */
/**
 * @typedef {Object} PolygonData - соответствует типу PolygonData в C#-коде
 * @property {Point} point - координаты контрольной точки
 * @property {Point[]} polygon - массив вершин многоугольника
 * @property {String} name - название точки
 */
/**
 * Функция отправляет данные о многоугольнике и контрольной точке на сервер.
 * @param {PolygonData} data
 */
function sendData(data) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/poly/data", true);
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.onreadystatechange = function () {
        if (xhr.readyState == XMLHttpRequest.DONE && xhr.status == 200) {
            let response = JSON.parse(xhr.responseText);
            addResultLine(response);
        }
    };
    xhr.send(JSON.stringify(data));
}

/**
 * Функция для отрисовки многоугольника в графической области
 * @param {Object} e - объект события щелчка мыши по графической области
 */
function drawPoly(e) {
    // отрисовка вершины многоугольника на экране
    let bounding = e.currentTarget.getBoundingClientRect();
    let x = e.x - bounding.x,
        y = e.y - bounding.y;
    svg
        .circle(radius * 2)
        .fill("#0f0")
        .move(x - radius, y - radius);
    // отрисовка линии, соединяющей текущую вершину с предыдущей
    if (points.length > 0) {
        let prev = points[points.length - 1];
        svg.line(prev.x, prev.y, x, y).stroke({ color: "#0f0", width: 2 });
    }
    points.push({ x: x, y: y });
}

/**
 * Функция для отрисовки контрольной точки в графической области.
 * @param {Object} e - объект события щелчка мыши по графической области
 */
function setPoint(e) {
    // Отрисовка точки на экране
    let bounding = e.currentTarget.getBoundingClientRect();
    let x = e.x - bounding.x,
        y = e.y - bounding.y;
    svg
        .circle(radius * 2)
        .fill("#f00")
        .move(x - radius, y - radius);
    pointIndex++;
    let name = pointIndex.toString();
    svg.text(name).move(x, y);

    // Отправка данных на сервер
    sendData({ polygon: points, point: { x: x, y: y }, name: name });
}

// Обработка события щелчка мыши по графической области
svg.click((e) => {
    if (!isDone) {
        drawPoly(e);
    } else {
        setPoint(e);
    }
});

// Обработка события щелчка мыши по кнопке Замкнуть
let doneButton = document.getElementById("done");
doneButton.onclick = (e) => {
    if (points.length === 0) {
        return;
    }

    // отрисовка линии, соединяющей текущую вершину с первой
    svg
        .line(
            points[points.length - 1].x,
            points[points.length - 1].y,
            points[0].x,
            points[0].y
        )
        .stroke({ color: "#0f0", width: 2 });
    isDone = true;
    doneButton.disabled = true;
};

// Обработка события щелчка мыши по кнопке Очистить
let resetButton = document.getElementById("reset");
resetButton.onclick = (e) => {
    if (points.length > 0) {
        for (let element of svg.children()) {
            element.remove();
        }
        points = [];
    }
    isDone = false;
    let resultsBlock = document.getElementById("results");
    resultsBlock.innerHTML="";
    pointIndex = 0;
    doneButton.disabled = false;
};
