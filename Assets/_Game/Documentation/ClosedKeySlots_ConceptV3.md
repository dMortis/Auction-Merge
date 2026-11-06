# Карточки коллекций по концепту V3

> Историческая версия с логикой. По следующему запросу пользователя все пользовательские скрипты и компоненты удалены из активного проекта. Актуальное состояние описано в `README_VisualOnly.md`; поля Found Count и компонента Collection Reward View больше нет.

Референс: `F:/Playable Ads/Storage Auction 3/Storage_Auction_3_Concept_Collections_Progress_v3_ClosedKeySlots.png`.

В сцене `Assets/_Game/Scenes/StorageAuction3_PlayableAd.unity` собраны компактные деревянные ниши с золотой рамкой, короткие ключи и гладкие круглые точки. Показательный прогресс совпадает с концептом: синяя коллекция 3/3, зелёная 2/3, красная 1/3. Иллюстрации коллекций остаются отдельными Image и заменяются независимо.

## Готовые элементы

- Вся группа карточек: `Assets/_Game/Prefabs/UI/Collections/Collections_ConceptV3.prefab`.
- Варианты прогресса 0/3–3/3: `Assets/_Game/Prefabs/UI/Collections/Rewards/CollectionReward_<Color>_<Count>of3.prefab`.
- Чистый фон карточки: `Assets/_Game/Art/UI/Collections/CollectionCard_Background.png`.
- Отдельный фон с деревянной нишей снизу: `Assets/_Game/Art/UI/Collections/CollectionCard_Background_Closed.png`.
- Закрытая ниша и открытые ниши трёх цветов: `Assets/_Game/Art/UI/Collections/Rewards/Slots/`.
- Точки: `Assets/_Game/Art/UI/Collections/Rewards/Dots/`.
- Компактные ключи: `Assets/_Game/Art/Items/LocksAndKeys/Key_Blue.png`, `Key_Green.png`, `Key_Red.png`.

Исходные PNG также сохранены в `F:/Playable Ads/Storage Auction 3/UI/Collections/`, `UI/CollectionRewards/` и `Locks_Keys_Icons/`.

## Как переключать состояние

На объекте `CollectionReward_Blue`, `CollectionReward_Green` или `CollectionReward_Red` есть компонент **Collection Reward View**. Поле **Found Count** принимает значения от 0 до 3 и работает также в режиме редактирования.

- 0/3–2/3: ниша закрыта, настоящий ключ полностью скрыт и не принимает ввод.
- 3/3: все три точки светятся, появляется открытая ниша и ключ.
- В режиме игры переход к 3/3 сопровождается коротким плавным проявлением; после него появляется рука. В редакторе рука скрыта, как на концепте V3.

Игровая логика передаёт прогресс через `CollectionRewardView.SetFoundCount(int)` или `SetCompletedItems(bool, bool, bool)`. Этот компонент отвечает за отображение награды. Система поиска/слияния предметов и перетаскивание ключа к замку подключаются отдельно.

Все элементы расположены относительно `CollectionArtSpace`, который совпадает с областью фонового спрайта при Preserve Aspect. Ниша не растягивается по ширине экрана отдельно от карточки.

## Проверка и резервные копии

Проверены все четыре значения прогресса для всех трёх цветов: видимость ключа, закрытой и открытой ниши, заполнение точек и разрешение ввода. В Play Mode проверены переход 2/3 → 3/3, завершение проявления, появление руки и повторное закрытие при возврате к 2/3. Ошибок и предупреждений Unity нет.

Предыдущие PNG, их настройки и префабы сохранены в `F:/Unity Projects/Storage Auction 3/_Archive/ClosedKeySlots_ConceptV3_20261027/`. Сцена до изменений: `Assets/_Game/Scenes/Backups/BeforeClosedKeySlots_20261027_020207.unity`.

Художественные задания для встроенного ImageGen: `F:/Playable Ads/Storage Auction 3/UI/CollectionRewards/ConceptV3_AssetPrompts.md`.
