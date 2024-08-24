using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    enum SpellPreviewType { None,Projectile,AreaOfEffect }

    enum SpellCastPosition { Player,Mouse }

    public bool CanCast { get { return _canCast; } }
    private bool _canCast = true;

    [Header("Info")]
    [SerializeField] private bool _autoDestroy = false;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _cooldown = 2f;
    [SerializeField] private float _height = 1f;
    [SerializeField] private GameObject _spellPrefab;
    [SerializeField] private SpellCastPosition _castPosition = SpellCastPosition.Player;

    [Header("Animation")]
    [SerializeField] private float _animationDuration = 1f;
    [SerializeField] private float _animationSpellMoment = 0f;

    [Header("Preview Zone")]
    [SerializeField] private GameObject _spellPreviewIndicatorPrefab = null;
    private GameObject _spellPreviewIndicator;
    [SerializeField] private SpellPreviewType _spellZoneType = SpellPreviewType.None;
    [SerializeField] private Material indicatorActiveMaterial;
    [SerializeField] private Material indicatorInactiveMaterial;

    public float Cast(Vector3 castLocation)
    {
        if (CanCast)
        {
            StartCoroutine(PerformSpell(castLocation));
            return _animationDuration;
        }
        else
        {
            return -1f;
        }
    }

    public void ShowPreview()
    {
        if (_spellZoneType != SpellPreviewType.None)
        {
            _spellPreviewIndicator = Instantiate(_spellPreviewIndicatorPrefab, transform);
            _spellPreviewIndicator.SetActive(true);
        }
    }

    public void Update()
    {
        if (_spellZoneType != SpellPreviewType.None && _spellPreviewIndicator != null)
        {
            UpdateSpellCastZone();
        }
    }

    private void UpdateSpellCastZone()
    {
        switch (_spellZoneType)
        {
            case SpellPreviewType.AreaOfEffect:
                UpdateMaterial();
                _spellPreviewIndicator.transform.position = MouseIndicator.GetMouseWorldPosition();
                break;
            case SpellPreviewType.Projectile:
                UpdateMaterial();
                Quaternion rotation = Quaternion.LookRotation(MouseIndicator.GetMouseWorldPosition() - transform.position);
                rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
                _spellPreviewIndicator.transform.rotation = Quaternion.Lerp(rotation, _spellPreviewIndicator.transform.rotation, 0);
                break;
        }
    }

    IEnumerator Cooldown()
    {
        _canCast = false;
        float cooldownRemaining = _cooldown;

        while (cooldownRemaining >= 0)
        {
            cooldownRemaining -= Time.deltaTime;
            yield return null;
        }

        _canCast = true;
    }

    IEnumerator PerformSpell(Vector3 castLocation)
    {
        StartCoroutine(Cooldown());

        GameObject spellPrefab;

        if (_castPosition == SpellCastPosition.Mouse)
        {
            castLocation = MouseIndicator.GetMouseWorldPosition();
        }
        castLocation.y += _height;

        if (_animationSpellMoment == _animationDuration)
        {
            yield return new WaitForSeconds(_animationDuration);

            spellPrefab = Instantiate(_spellPrefab, castLocation, transform.rotation);

        }
        else
        {
            yield return new WaitForSeconds(_animationSpellMoment);

            spellPrefab = Instantiate(_spellPrefab, castLocation, transform.rotation);

            yield return new WaitForSeconds(_animationDuration - _animationSpellMoment);
        }

        if (!_autoDestroy)
        {
            yield return new WaitForSeconds(_duration);
            Destroy(spellPrefab);
        }
    }

    public void HideCastZone()
    {
        Destroy(_spellPreviewIndicator);
    }

    private void UpdateMaterial()
    {
        if (_canCast)
        {
            _spellPreviewIndicator.GetComponent<Canvas>().transform.Find("Image").GetComponent<Image>().material = indicatorActiveMaterial;
        }
        else
        {
            _spellPreviewIndicator.GetComponent<Canvas>().transform.Find("Image").GetComponent<Image>().material = indicatorInactiveMaterial;
        }
    }
}
