﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EventCharacterBase(CharacterBase character);
public delegate void EventCharacterBaseDouble(CharacterBase character, CharacterBase character2);
public delegate void EventAttackManager(AttackManager attack);
public delegate void EventAttackSubManager(AttackSubManager attack);