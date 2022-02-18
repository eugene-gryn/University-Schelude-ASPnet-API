### **Идея проекта:**
#### Приложение для уведомлений студента о парах.

### **Описание:**
#### Цель этого приложения уведомлять студентов до начала пары, та уточнять её домашнее задание и указывать его приоритет.

### **Функционал движка:** 0.1(beta)
	- Профиль пользователя
		- Аватар
			- Установка по умолчанию (картинка)
			- Обновить (путь к файлу)
		- Имя
			- Поменять (строка)
		- Логин
			- При регистрации создать
		- Пароль
			- При регистрации создать
			- Возможность поменять
		- Настройки
			- Присылать ли уведомления?
			- ...
	- Создание пользователя или доступ к существующему (БАЗЫ ДАННЫХ)
		- Логин (посмотреть как безпастно сделать)
			- Проверка через удаленную базу данных
			- Хеширование
		- Регистрации
			- Проверка на существующего пользователя
			- Добавление в удаленную базу данных
			- Привязка до акканта телеграмм (отдельный бот)
		- Восстановление пароля
			- Восстановление через запрос к боту
			- Установка нового пароля
	- Управление рассписанием
		- Загрузка рассписания
			- Excel загрузка
				- Загрузка через Excel файл определенного формата
			- PDF загрузка
				- Загрузка готового рассписания с сайта
			- Ручной ввод (отдельный предмет)
				- время
				- название
				- аудитории\гуглкласс
				- *предподавателя(не обезательно)
		- Отображение
			- Передача дня
			- Геттеры для пар в указаный день
			- Геттер для полного списка пар
		- Ближайшая пара
			- Получение пары, которая будет в ближайшее время
		- Накладка пар
			- Сделать так чтобы при добавлении пара не пересекалась с другими
		- Удаление
			- Удаление пары по указанию с PL (по обьекту)
		- Изменение
			- Изменение данных про пару (имя, время, ссылка\аудитори, препод)
	- Управление уведомленями
		- Виды
			- Уведомление про пару за 5 минут до начала
			- Уведомление про пару в начало
			- Уведомление в конец пары с вопросом по дз
			- Уведомление за день до конца дедлайна из дз
			- Уведомление про пропущенный дедлайн
		- Возможность выключить уведомление
			- Возможность выключить одно из уведомлений
			- Возможность выключить сразу все
	- Управление домашними заданиями
		- Содержание самого дз
			- Название предмета
				- Должно иметь ссылку на обьект предмета (по которому дз)
			- Текст
				- Текст самого домашнего задания
			- Дэдлайн
				- Крайний срок выполнения (дата и время)
			- Приоритет
				- Приоритет выполнения (по нему модет сортироватсья список)
			- Документ
				- Возможность прикрепить документ(любой файл) к домашнему заданию
		- Операции
			- Удаление дз
			- Выполнение дз (разниица в том что помещается в список выполненых дз)
			- Смена данных про дз
			- Переопределение сроков (сменить дедлайн, отложить)
		

### **Функционал windows приложения**
	Пусто

### **Функционал тг бота**
	Пусто

	


### Проект составляет из себя 3 основные части:
	Main desktop - WPF windows приложение
	Sheldue logic - Движок программы
	Telegram bot - Бот мессенжера телеграмм