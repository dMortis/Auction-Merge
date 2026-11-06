# Награда коллекции: точки и ключ

Обновлённый макет: `F:/Playable Ads/Storage Auction 3/Storage_Auction_3_Concept_Collections_Progress_v2.png`.

Три точки расположены над нишей, ключ целиком находится внутри нижней золотой рамки карточки. Голубой, зелёный и красный ключи имеют одну форму головки и одинаковые пропорции.

## Ассеты

- Ключи: `Assets/_Game/Art/Items/LocksAndKeys/Key_Blue.png`, `Key_Green.png`, `Key_Red.png`.
- Точки: `Assets/_Game/Art/UI/Collections/Rewards/Dots/`.
- Белая рука: `Assets/_Game/Art/UI/Collections/Rewards/Tutorial/TutorialHand_White.png`.
- Префабы: `Assets/_Game/Prefabs/UI/Collections/Rewards/`.

В папке точек каждый цвет имеет три отдельных PNG: `Empty` — пустая, `Filled` — заполненная, `Complete` — заполненная со свечением. Их прозрачные полотна одинаковые: 512×512. Файлы `Progress_<Color>_3of3_Glow.png` содержат готовые три светящиеся точки в ряд.

Для каждого цвета есть четыре префаба `CollectionReward_<Color>_0of3` ... `_3of3`. Вариант 3/3 показывает три светящиеся точки и яркий ключ; варианты 0/3–2/3 показывают приглушённый ключ. Рука доступна отдельным префабом `TutorialHand_White`.

## Размещение в сцене

Сцена: `Assets/_Game/Scenes/StorageAuction3_PlayableAd.unity`.

Новые блоки находятся в `Canvas / Safe Area / Collections / Collection01–03`. Состояния при установке соответствуют уже выбранным иллюстрациям: Skeleton State_06 → 2/3, Detective State_07 → 3/3, Concert State_03 → 1/3. Рука указывает на готовый зелёный ключ.

Это визуальные префабы состояний. Автоматическое заполнение, проверку собранных предметов, перетаскивание ключа и открытие замка предстоит связать с игровой логикой.

Расположение учитывает внутренние поля спрайта `CollectionCard_Background` при включённом Preserve Aspect. При замене фона карточки нужно проверить положение нижней рамки.

## Резервные копии

- Старые ключи проекта, их `.meta` и исходные ключи: `_Archive/CollectionRewards_Before_20261027/`.
- Сцена до добавления UI: `Assets/_Game/Scenes/Backups/BeforeCollectionRewardsV2_20261027_004700.unity`.
- Сцена перед выравниванием: `Assets/_Game/Scenes/Backups/BeforeRewardInsetAlign_20261027_005245.unity`.
- Префабы перед выравниванием: `_Archive/CollectionRewards_InsetAlign_20261027_005245/`.

Изображения созданы встроенным ImageGen. Параметры художественного задания сохранены рядом с исходниками: `F:/Playable Ads/Storage Auction 3/UI/CollectionRewards/PROMPTS.md`.
